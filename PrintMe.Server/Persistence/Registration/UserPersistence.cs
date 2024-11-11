using PrintMe.Server.Models.Registration;

namespace PrintMe.Server.Persistence.Registration;

public class UserPersistence
{
    private readonly AppDbContext _context;

    public UserPersistence(AppDbContext context)
    {
        _context = context;
    }

    public void SaveUser(UserRegistrationInfo userInfo)
    {
        _context.Users.Add(userInfo);
        _context.SaveChanges();
    }
}