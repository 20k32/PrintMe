namespace PrintMe.Server.Models.Api.ApiRequest;

public sealed class UserRegisterRequest : INullCheck
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    
    public UserRegisterRequest(string firstName, string lastName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public bool IsNull() => string.IsNullOrWhiteSpace(FirstName)
                            || string.IsNullOrWhiteSpace(LastName)
                            || string.IsNullOrWhiteSpace(Email)
                            || string.IsNullOrWhiteSpace(Password);
}