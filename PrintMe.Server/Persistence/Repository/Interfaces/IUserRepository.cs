using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> UpdateUserByEmailAsync(string email, User newEntity);
    Task<User> GetUserByIdAsync(int id);
    Task<User> UpdateUserByIdAsync(int id, User newEntity);
    Task<bool> CheckIfRoleExistsAsync(string roleName);
    Task<UserRole> GetRoleIdByNamesAsync(string roleName);
    string GetUserRole(int userId);
    Task<User> GetUserByTokenAsync(string token);
}