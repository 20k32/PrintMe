using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IRolesRepository
{
    Task<UserRole> GetByRoleNameAsync(string roleName);
}