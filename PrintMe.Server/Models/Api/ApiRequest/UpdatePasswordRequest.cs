using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.DTOs.UserDto;

namespace PrintMe.Server.Models.Api.ApiRequest
{
    public class UpdatePasswordRequest : INullCheck
    {
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }
        public NoPasswordUserDto UserWithNoPassword { get; init; }

        public bool IsNull() => string.IsNullOrWhiteSpace(OldPassword)
                                || string.IsNullOrWhiteSpace(NewPassword)
                                || UserWithNoPassword is null;
    }
}