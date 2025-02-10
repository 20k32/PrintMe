using System.Net;
using System.Net.Mail;
using PrintMe.Server.Controllers;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database;

internal sealed class VerificationService
{
    private readonly UserRepository _userRepository;
    private readonly SmtpClient _smtpClient; 
    private readonly MailAddress _fromAddress;
    
    public VerificationService(UserRepository userRepository)
    {
        _userRepository = userRepository;
        
        _smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("MAIL_USERNAME"), Environment.GetEnvironmentVariable("MAIL_PASSWORD")),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false
        };
        _fromAddress = new MailAddress(Environment.GetEnvironmentVariable("MAIL_USERNAME"));
    }
    public async Task SendEmailVerificationAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        
        if (user is null)
        {
            throw new NotFoundUserInDbException();
        }
        
        user.ConfirmationToken = Guid.NewGuid().ToString();     
        
        var mailMessage = new MailMessage
        {
            From = _fromAddress,
            Subject = "Verify your account",
            Body = $"Hello, please verify your account by clicking the link below:</br>http://localhost:5173/verify?token={user.ConfirmationToken}",
            IsBodyHtml = true
        };
        
        mailMessage.To.Add(user.Email);
        
        await _smtpClient.SendMailAsync(mailMessage);
        
        user.IsVerified = false;
        
        await _userRepository.UpdateUserByIdAsync(user.UserId, user);
    }
    
    public async Task VerifyEmailAsync(string token)
    {
        var user = await _userRepository.GetUserByTokenAsync(token);
        
        if (user is null)
        {
            throw new NotFoundUserInDbException();
        }
        if (user.ConfirmationToken != token)
        {
            throw new InvalidUUIDTokenException();
        }
        user.IsVerified = true;
        user.ConfirmationToken = null;
        await _userRepository.UpdateUserByIdAsync(user.UserId, user);
    }
}