using System.Text.Json.Serialization;

namespace PrintMe.Server.Models.Authentication;

public sealed class UserAuthRequest
{
    public string Email { get; init; }
    public string Password { get; init; }

    public UserAuthRequest(string email, string password) 
        => (Email, Password) = (email, password);
}