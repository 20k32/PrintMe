using System.Text.Json.Serialization;

namespace PrintMe.Server.Models.DTOs.UserDto
{
    public class NoPasswordUserDto : INullCheck
    {
	    [JsonIgnore]
        public int UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        [JsonIgnore]
        public int? UserStatusId { get; init; }
        public bool? ShouldHidePhoneNumber { get; init; }
        public string Description { get; init; }
        [JsonIgnore]
        public string UserRole { get; init; }

        public virtual bool IsNull() => UserId == default
                                || string.IsNullOrWhiteSpace(FirstName)
                                || string.IsNullOrWhiteSpace(LastName)
                                || string.IsNullOrWhiteSpace(Email)
                                || string.IsNullOrWhiteSpace(UserRole);
    }
}