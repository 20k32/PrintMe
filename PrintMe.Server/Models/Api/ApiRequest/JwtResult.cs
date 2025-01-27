using System.Data.SqlTypes;

namespace PrintMe.Server.Models.Api.ApiRequest;

public sealed class JwtResult(string token, string refreshToken) : INullCheck
{
    public string AccessToken { get; init; } = token;
    public string RefreshToken { get; init; } = refreshToken;

    public bool IsNull() => string.IsNullOrEmpty(AccessToken) || string.IsNullOrEmpty(RefreshToken);
}