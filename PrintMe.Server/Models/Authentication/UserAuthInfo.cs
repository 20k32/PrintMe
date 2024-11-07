using System.Text.Json.Serialization;

namespace PrintMe.Server.Models.Authentication;

public sealed class UserAuthInfo
{
    public string Name { get; init; }
    public string Role { get; init; }

    public UserAuthInfo(string name, string role) 
        => (Name, Role) = (name, role);
}