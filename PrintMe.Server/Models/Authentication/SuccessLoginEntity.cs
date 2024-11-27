namespace PrintMe.Server.Models.Authentication
{
    public sealed class SuccessLoginEntity
    {
        public string Email { get; init; }
        public int Id { get; init; }
        public string Role { get; init; }

        public SuccessLoginEntity(int id, string email, string role) 
            => (Id, Email, Role) = (id, email, role);
    }
}