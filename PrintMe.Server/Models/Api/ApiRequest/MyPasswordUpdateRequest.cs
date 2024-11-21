namespace PrintMe.Server.Models.Api.ApiRequest
{
    public class MyPasswordUpdateRequest : INullCheck
    {
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }

        public bool IsNull() => string.IsNullOrWhiteSpace(OldPassword)
                                || string.IsNullOrWhiteSpace(NewPassword);
    }
}