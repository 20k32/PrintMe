namespace PrintMe.Server.Models.Api.ApiResult.Auth
{
    public sealed class TokenResult : ResultBase
    {
        public string Token { get; init; }
        
        public TokenResult(string token, string message, int statusCode) : base(message, statusCode) 
            => Token = token;
    }
}