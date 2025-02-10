namespace PrintMe.Server.Models.DTOs.UserDto
{
    public class NoPasswordUserDto : INullCheck
    {
        public int UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string PhoneNumber { get; init; }
        public int? UserStatusId { get; init; }
        public bool? ShouldHidePhoneNumber { get; init; }
        public string Description { get; init; }
        public string UserRole { get; init; }
        
        public bool isVerified { get; set; }

        public virtual bool IsNull() => UserId == default
                                || string.IsNullOrWhiteSpace(FirstName)
                                || string.IsNullOrWhiteSpace(LastName)
                                || string.IsNullOrWhiteSpace(Email)
                                || string.IsNullOrWhiteSpace(UserRole);
    }
}