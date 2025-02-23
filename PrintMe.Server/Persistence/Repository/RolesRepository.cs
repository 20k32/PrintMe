using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    internal class RolesRepository
    {
        private PrintMeDbContext _dbContext;

        public RolesRepository(PrintMeDbContext dbContext) => _dbContext = dbContext;
        
        public async Task<UserRole> GetByRoleNameAsync(string roleName) => await _dbContext.UserRoles
            .AsQueryable()
            .FirstOrDefaultAsync(existing => existing.UserRoleName.Equals(roleName));
    }
}