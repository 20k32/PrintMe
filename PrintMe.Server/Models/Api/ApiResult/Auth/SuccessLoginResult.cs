namespace PrintMe.Server.Models.Api.ApiResult.Auth
{
    public sealed class SuccessLoginResult
    {
        public string Email { get; init; }
        public int Id { get; init; }
        public string Role { get; init; }

        public SuccessLoginResult(int id, string email, string role) 
            => (Id, Email, Role) = (id, email, role);
    }
}