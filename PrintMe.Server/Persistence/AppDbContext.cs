using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.Registration;

namespace PrintMe.Server.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<UserRegistrationInfo> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("printme_db@localhost"); 
    }
}
