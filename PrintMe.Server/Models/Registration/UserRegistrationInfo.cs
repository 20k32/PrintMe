namespace PrintMe.Server.Models.Registration;

public class UserRegistrationInfo
{
    public int Id { get; init; }
    public string Email { get; init; }
    public string FirstName {get; init;}
    public string LastName {get; init;}
    public string HashedPassword { get; init; }
    public string Salt { get; init; }
    // public UserRegistrationInfo(string email, string firstName, string lastName, string hashedPassword, string salt)
    // {
    //     Email = email;
    //     FirstName = firstName;
    //     LastName = lastName;
    //     HashedPassword = hashedPassword;
    //     Salt = salt;
    // }
    public UserRegistrationInfo(string email, string firstName, string lastName, string hashedPassword, string salt)
    => (Email, FirstName, LastName, HashedPassword, Salt) = (email, firstName, lastName, hashedPassword, salt);
}