using PrintMe.Server.Models.DTOs;

namespace PrintMe.Server.Models.Api.ApiResult.Common
{
    public class UserResult : ResultBase
    {
        public PasswordUserDto PasswordUserDto { get; init; }
        
        public UserResult()
        { }

        public UserResult(PasswordUserDto passwordUserDto, string message, int statusCode) : base(message, statusCode) 
            => PasswordUserDto = passwordUserDto;
    }
}