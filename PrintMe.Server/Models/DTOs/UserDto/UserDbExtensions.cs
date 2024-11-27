using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs.UserDto
{
    public static class UserDbExtensions
    {
        public static PasswordUserDto MapToPasswordUserDto(this NoPasswordUserDto noPasswordUser, string password, string salt) => new()
        {
            UserId = noPasswordUser.UserId,
            FirstName = noPasswordUser.FirstName,
            LastName = noPasswordUser.LastName,
            Email = noPasswordUser.Email,
            PhoneNumber = noPasswordUser.PhoneNumber,
            UserStatusId = noPasswordUser.UserStatusId,
            ShouldHidePhoneNumber = noPasswordUser.ShouldHidePhoneNumber,
            Description = noPasswordUser.Description,
            Password = password,
            PasswordSalt = salt,
            UserRole = noPasswordUser.UserRole,
        };
        
        public static NoPasswordUserDto MapToNoPasswordUserDto(this User user) => new()
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserStatusId = user.UserStatusId,
            ShouldHidePhoneNumber = user.ShouldHidePhoneNumber,
            Description = user.Description,
            UserRole = user.UserRole.UserRoleName
        };
        
        public static PasswordUserDto MapToPasswordUserDto(this User user) => new()
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            UserStatusId = user.UserStatusId,
            ShouldHidePhoneNumber = user.ShouldHidePhoneNumber,
            Description = user.Description,
            Password = user.Password,
            PasswordSalt = user.PasswordSalt,
            UserRole = user?.UserRole?.UserRoleName ?? string.Empty
        };

        public static User MapToUser(this NoPasswordUserDto noPasswordUser) => new()
        {
            UserId = noPasswordUser.UserId,
            FirstName = noPasswordUser.FirstName,
            LastName = noPasswordUser.LastName,
            Email = noPasswordUser.Email,
            PhoneNumber = noPasswordUser.PhoneNumber,
            UserStatusId = noPasswordUser.UserStatusId,
            ShouldHidePhoneNumber = noPasswordUser.ShouldHidePhoneNumber,
            Description = noPasswordUser.Description,
        };
        
        public static User MapToUser(this PasswordUserDto passwordUserDto, UserRole role) => new()
        {
            UserId = passwordUserDto.UserId,
            FirstName = passwordUserDto.FirstName,
            LastName = passwordUserDto.LastName,
            Email = passwordUserDto.Email,
            PhoneNumber = passwordUserDto.PhoneNumber,
            UserStatusId = passwordUserDto.UserStatusId,
            ShouldHidePhoneNumber = passwordUserDto.ShouldHidePhoneNumber,
            Description = passwordUserDto.Description,
            Password = passwordUserDto.Password,
            PasswordSalt = passwordUserDto.PasswordSalt,
            UserRole = role
        };
    }
}