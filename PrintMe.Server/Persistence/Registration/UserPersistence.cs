using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Entities;
using PrintMe.Server.Models.Registration;

namespace PrintMe.Server.Persistence.Registration;

public class UserPersistence
{
    private readonly UserContext _context;

    public UserPersistence(UserContext context)
    {
        _context = context;
    }

    public void SaveUser(UserRegistrationInfo userInfo)
    {
        _context.Add(userInfo);
        _context.SaveChanges();
        var user = _context.Users.Find(userInfo.Id);
    }
    
}