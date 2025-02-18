using System.Data;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using PrintMe.Server.Logic.Authentication;
using PrintMe.Server.Logic.Helpers;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.Authentication;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.DTOs.UserDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;
using PrintMe.Server.Constants;

namespace PrintMe.Server.Logic.Services.Database
{
    internal sealed partial class UserService
    {
        private readonly UserRepository _repository;
        private readonly TokenGenerator _tokenGenerator;

        public UserService(UserRepository repository, TokenGenerator tokenGenerator) =>
            (_repository, _tokenGenerator) = (repository, tokenGenerator);

        public async Task AddUserAsync(UserRegisterRequest user)
        {
            if (!EmailRegex().IsMatch(user.Email))
            {
                throw new InvalidEmailFormatException();
            }
            var salt = SecurityHelper.GenerateSalt();
            var hashedPassword = SecurityHelper.HashPassword(user.Password, salt);
            var userRoleId = DbConstants.UserRole.Dictionary[DbConstants.UserRole.User];
            var activeStatusId = DbConstants.UserStatus.Dictionary[DbConstants.UserStatus.Active];

            var userRaw = new User
            {
                Email = user.Email.ToLower(),
                Password = hashedPassword,
                PasswordSalt = salt,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserStatusId = activeStatusId,
                UserRoleId = userRoleId,
                PhoneNumber = "",
                ShouldHidePhoneNumber = true,
                Description = "",
                ConfirmationToken = null,
                RefreshToken = _tokenGenerator.GenerateRefreshToken(),
                IsVerified = false
            };
            await _repository.AddUserAsync(userRaw);
        }
        public async Task<PasswordUserDto> GetUserByEmailAsync(string email)
        {
            var userRaw = await _repository.GetUserByEmailAsync(email);

            if (userRaw is null)
            {
                throw new NotFoundUserInDbException();
            }

            return userRaw.MapToPasswordUserDto();
        }

        public async Task<PasswordUserDto> GetUserByIdAsync(int id)
        {
            var userRaw = await _repository.GetUserByIdAsync(id);

            if (userRaw is null)
            {
                throw new NotFoundUserInDbException();
            }

            return userRaw.MapToPasswordUserDto();
        }

        public async Task<PasswordUserDto> UpdateUser(string email, NoPasswordUserDto user)
        {
            PasswordUserDto result;

            var userRaw = user.MapToUser();

            var updateResult = await _repository.UpdateUserByEmailAsync(email, userRaw);

            if (updateResult is null)
            {
                throw new NotFoundUserInDbException();
            }

            result = updateResult.MapToPasswordUserDto();

            return result;
        }

        public async Task<PasswordUserDto> UpdateUser(int id, NoPasswordUserDto user)
        {
            PasswordUserDto result;

            var userRaw = user.MapToUser();

            var updateResult = await _repository.UpdateUserByIdAsync(id, userRaw);

            if (updateResult is null)
            {
                throw new NotFoundUserInDbException();
            }

            result = updateResult.MapToPasswordUserDto();

            return result;
        }

        public async Task<PasswordUserDto> UpdateUserPasswordAsync(UpdatePasswordRequest passwordRequest)
        {
            PasswordUserDto result;

            var dbUser =
                await _repository.GetUserByIdAsync(passwordRequest.UserWithNoPassword.UserId);

            if (dbUser is null)
            {
                throw new NotFoundUserInDbException();
            }

            var oldPasswordHash = SecurityHelper.HashPassword(passwordRequest.OldPassword, dbUser.PasswordSalt);

            if (!oldPasswordHash.Equals(dbUser.Password))
            {
                throw new IncorrectPasswordException();
            }

            var newPasswordSalt = SecurityHelper.GenerateSalt();

            var newPasswordHash = SecurityHelper.HashPassword(
                passwordRequest.NewPassword, newPasswordSalt);

            dbUser.Password = newPasswordHash;
            dbUser.PasswordSalt = newPasswordSalt;

            var updateResult = await _repository.UpdateUserByIdAsync(dbUser.UserId, dbUser);

            if (updateResult is null)
            {
                throw new DatabaseInternalException();
            }

            result = updateResult.MapToPasswordUserDto();

            return result;
        }

        public async Task<PasswordUserDto> UpdateUserPasswordAsync(int id, MyPasswordUpdateRequest passwordRequest)
        {
            PasswordUserDto result;

            var dbUser =
                await _repository.GetUserByIdAsync(id);

            if (dbUser is null)
            {
                throw new NotFoundUserInDbException();
            }

            var oldPasswordHash = SecurityHelper.HashPassword(passwordRequest.OldPassword, dbUser.PasswordSalt);

            if (!oldPasswordHash.Equals(dbUser.Password))
            {
                throw new IncorrectPasswordException();
            }

            var newPasswordSalt = SecurityHelper.GenerateSalt();

            var newPasswordHash = SecurityHelper.HashPassword(
                passwordRequest.NewPassword, newPasswordSalt);

            dbUser.Password = newPasswordHash;
            dbUser.PasswordSalt = newPasswordSalt;

            var updateResult = await _repository.UpdateUserByIdAsync(dbUser.UserId, dbUser);

            if (updateResult is null)
            {
                throw new DatabaseInternalException();
            }

            result = updateResult.MapToPasswordUserDto();

            return result;
        }

        public async Task<JwtResult> GenerateTokenAsync(UserAuthRequest authRequest)
        {
            var dbUser = await _repository.GetUserByEmailAsync(authRequest.Email);

            if (dbUser is null)
            {
                throw new NotFoundUserInDbException();
            }
            var hashedPassword = SecurityHelper.HashPassword(authRequest.Password, dbUser.PasswordSalt);

            if (!hashedPassword.Equals(dbUser.Password))
            {
                throw new IncorrectPasswordException();
            }

            var loginResult = new SuccessLoginEntity(dbUser.UserId, authRequest.Email, _repository.GetUserRole(dbUser.UserId));
            var tokenResult = _tokenGenerator.GetForSuccessLoginResult(loginResult);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();

            return new JwtResult(tokenResult, refreshToken);
        }

        [GeneratedRegex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
        private static partial Regex EmailRegex();
        
        public async Task<JwtResult> RefreshTokenAsync(JwtResult jwtResult)
        {
            var claims = _tokenGenerator.GetPrincipalFromExpiredToken(jwtResult.AccessToken);
            
            if (claims is null)
            {
                throw new NotFoundClaimsException();
            }

            var userId = claims.Claims.FirstOrDefault(claim => claim.Type == CustomClaimTypes.USER_ID)
                ?.Value;
            var idInteger = int.Parse(userId);
            var dbUser = await _repository.GetUserByIdAsync(idInteger);

            if (dbUser is null)
            {
                throw new NotFoundUserInDbException();
            }

            var successLoginEntity = new SuccessLoginEntity(dbUser.UserId, dbUser.Email,
                claims.Claims.First(claim => claim.Type == ClaimTypes.Role).Value);
            
            var newAccessToken = _tokenGenerator.GetForSuccessLoginResult(successLoginEntity);

            
            var updatedUser =  await _repository.UpdateUserByIdAsync(dbUser.UserId, dbUser);

            if (updatedUser is null)
            {
                throw new DatabaseInternalException();
            }
            
            return new JwtResult(newAccessToken, dbUser.RefreshToken);
        }
    }
}