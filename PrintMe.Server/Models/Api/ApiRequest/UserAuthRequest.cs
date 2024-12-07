namespace PrintMe.Server.Models.Api.ApiRequest;

public sealed class UserAuthRequest : INullCheck
{
    public string Email { get; init; }
    public string Password { get; init; }
    public UserAuthRequest(string email, string password)
        => (Email, Password) = (email, password);

    public bool IsNull() => string.IsNullOrWhiteSpace(Email)
                            || string.IsNullOrWhiteSpace(Password);
}