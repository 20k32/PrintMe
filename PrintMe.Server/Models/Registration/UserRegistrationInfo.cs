using System.ComponentModel.DataAnnotations;

namespace PrintMe.Server.Models.Registration;

public class UserRegistrationInfo
{
    [Required]
    public int Id { get; init; }
    [Required]
    public string Email { get; init; }
    [Required]
    public string FirstName {get; init;}
    [Required]
    public string LastName {get; init;}
    [Required]
    public string HashedPassword { get; init; }
    [Required]
    public string Salt { get; init; }
    public UserRegistrationInfo(string email, string firstName, string lastName, string hashedPassword, string salt)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        HashedPassword = hashedPassword;
        Salt = salt;
    }
}