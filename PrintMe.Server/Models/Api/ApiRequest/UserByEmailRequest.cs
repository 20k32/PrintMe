namespace PrintMe.Server.Models.Api.ApiRequest
{
    public sealed class UserByEmailRequest : INullCheck
    {
        public string Email { get; init; }

        public UserByEmailRequest()
        { }

        public bool IsNull() => string.IsNullOrWhiteSpace(Email);
    }
}