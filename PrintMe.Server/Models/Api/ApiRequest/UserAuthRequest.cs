namespace PrintMe.Server.Models.Api.ApiRequest;

public sealed class UserAuthRequest : INullCheck
{
    public string Email { get; init; }
    public string Password { get; init; }
    public string Role { get; init; }
    public UserAuthRequest(string email, string password, string role) 
        => (Email, Password, Role) = (email, password, role);

    public bool IsNull() => string.IsNullOrWhiteSpace(Email)
                            || string.IsNullOrWhiteSpace(Password)
                            || string.IsNullOrWhiteSpace(Role);
}