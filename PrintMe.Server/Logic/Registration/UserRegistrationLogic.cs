
using System.Text.RegularExpressions;
using PrintMe.Server.Models.Registration;
using PrintMe.Server.Persistence.Models;

namespace PrintMe.Server.Logic.Registration;

internal static class UserRegistrationLogic
{
    public static User CreateUser(UserRegistrationInfo userRegistration)
    {
        if (!Regex.IsMatch(userRegistration.Email, @"^[a-zA-Z0-9._]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            // Example Matches
            // exAmple46@example.com: Matches
            // user.name33@domain.co: Matches
            // user_name@sub.domain.com: Matches
            // Example Non-Matches
            // example@: Does not match (missing domain and TLD)
            // @example.com: Does not match (missing local part)
            // example@domain: Does not match (missing TLD)
            // example@domain..com: Does not match (invalid TLD)
        {
            throw new ArgumentException("Invalid email format");
        }
        var salt = SecurityHelper.GenerateSalt();
        var hashedPassword = SecurityHelper.HashPassword(userRegistration.Password, salt);
        var user = new User
        {
            Email = userRegistration.Email.ToLower(),
            Password = hashedPassword,
            PasswordSalt = salt,
            FirstName = userRegistration.FirstName,
            LastName = userRegistration.LastName,
            UserStatusId = 1,
            PhoneNumber = "",
            ShouldHidePhoneNumber = true,
            Description = ""
        };
        return user;
    }
}