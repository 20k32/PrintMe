using PrintMe.Server.Models.DTOs;

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