namespace PrintMe.Server.Logic.Services.Database.Interfaces;

public interface IVerificationService
{
    Task SendEmailVerificationAsync(int userId);
    Task VerifyEmailAsync(string token);
}