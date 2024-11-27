namespace PrintMe.Server.Models.DTOs.UserDto
{
    public class PasswordUserDto : NoPasswordUserDto
    {
        public string Password { get; init; }
        public string PasswordSalt { get; init; }

        public override bool IsNull()
        {
            return base.IsNull()
                   || string.IsNullOrWhiteSpace(Password)
                   || string.IsNullOrWhiteSpace(PasswordSalt);
        }
    }
}