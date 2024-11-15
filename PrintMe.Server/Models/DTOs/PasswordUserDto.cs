namespace PrintMe.Server.Models.DTOs
{
    public class PasswordUserDto : NoPasswordUserDto
    {
        public string Password { get; init; }
        public string PasswordSalt { get; init; }
    }
}