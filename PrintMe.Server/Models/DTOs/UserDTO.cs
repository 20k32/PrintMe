using PrintMe.Server.Persistence.Models;

namespace PrintMe.Server.Models.DTOs
{
    public class UserDTO
    {
        public int UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public int? UserStatusId { get; init; }
        public bool? ShouldHidePhoneNumber { get; init; }
        public string Description { get; init; }
        public string Password { get; init; }
        public string PasswordSalt { get; init; }
    }

    public static class UserDbExtensions
    {
        public static void MapToDTO(User user)
        {
            
        }
    }
}