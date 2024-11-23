namespace PrintMe.Server.Persistence.Entities
{
    public class UserRole
    {
        public int UserRoleId { get; set; }
        public string UserRoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}