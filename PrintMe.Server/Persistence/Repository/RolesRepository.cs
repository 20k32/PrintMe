using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Persistence.Repository
{
    internal class RolesRepository : IRolesRepository
    {
        private PrintMeDbContext _dbContext;

        public RolesRepository(PrintMeDbContext dbContext) => _dbContext = dbContext;
        
        public async Task<UserRole> GetByRoleNameAsync(string roleName) => await _dbContext.UserRoles
            .AsQueryable()
            .FirstOrDefaultAsync(existing => existing.UserRoleName.Equals(roleName));
    }
}