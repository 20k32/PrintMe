
using System.Text.RegularExpressions;
using PrintMe.Server.Persistence.Models;

namespace PrintMe.Server.Logic.Registration;

internal static class UserRegistrationLogic
{
    public static void CreateUser(string userEmail, string password, string firstName, string lastName)
    {
        // if (!Regex.IsMatch(userEmail, @"^[a-zA-Z0-9._]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
        //     // Example Matches
        //     // exAmple46@example.com: Matches
        //     // user.name33@domain.co: Matches
        //     // user_name@sub.domain.com: Matches
        //     // Example Non-Matches
        //     // example@: Does not match (missing domain and TLD)
        //     // @example.com: Does not match (missing local part)
        //     // example@domain: Does not match (missing TLD)
        //     // example@domain..com: Does not match (invalid TLD)
        // {
        //     throw new ArgumentException("Invalid email format");
        // }
        userEmail = userEmail.ToLower();
        var salt = SecurityHelper.GenerateSalt();
        var hashedPassword = SecurityHelper.HashPassword(password, salt);

//        return user;
    }
}