using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.UserDto;

namespace PrintMe.Server.Logic.Services.Database.Interfaces;

public interface IUserService
{
    Task AddUserAsync(UserRegisterRequest user);
    Task<PasswordUserDto> GetUserByEmailAsync(string email);
    Task<PasswordUserDto> GetUserByIdAsync(int id);
    Task<PasswordUserDto> UpdateUser(string email, NoPasswordUserDto user);
    Task<PasswordUserDto> UpdateUser(int id, NoPasswordUserDto user);
    Task<PasswordUserDto> UpdateUserPasswordAsync(UpdatePasswordRequest passwordRequest);
    Task<PasswordUserDto> UpdateUserPasswordAsync(int id, MyPasswordUpdateRequest passwordRequest);
    Task<JwtResult> GenerateTokenAsync(UserAuthRequest authRequest);
    Task<JwtResult> RefreshTokenAsync(JwtResult jwtResult);
    // [GeneratedRegex(@"^[a-zA-Z0-9._]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
    // Regex EmailRegex(); // private static partial
}