using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.Registration;

namespace PrintMe.Server.Entities;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
    }
    public DbSet<UserRegistrationInfo> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("DefaultConnection");
    }
}