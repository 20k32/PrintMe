using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Persistence.Repository
{
    internal class UserRepository : IUserRepository
    {
        private readonly IRolesRepository _rolesRepository;
        private readonly PrintMeDbContext _dbContext;

        public UserRepository(PrintMeDbContext dbContext, IRolesRepository rolesRepository) 
            => (_dbContext, _rolesRepository) = (dbContext, rolesRepository);

        public async Task AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public Task<User> GetUserByEmailAsync(string email) =>
            _dbContext
                .Users
                .AsQueryable()
                .Include(user => user.UserRole)
                .FirstOrDefaultAsync(user => user.Email.Equals(email));

        public async Task<User> UpdateUserByEmailAsync(string email, User newEntity)
        {
            var existingUser = await _dbContext
                .Users
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.Email == email);

            if (existingUser is null)
            {
                return null;
            }
            
            existingUser.FirstName = newEntity.FirstName;
            existingUser.LastName = newEntity.LastName;
            existingUser.Description = newEntity.Description;
            existingUser.PhoneNumber = newEntity.PhoneNumber;
            existingUser.ShouldHidePhoneNumber = newEntity.ShouldHidePhoneNumber;

            _dbContext.Users.Update(existingUser);
            
            await _dbContext.SaveChangesAsync();

            return existingUser;
        }
        
        
        public Task<User> GetUserByIdAsync(int id) =>
            _dbContext
                .Users
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.UserId ==  id);

        public async Task<User> UpdateUserByIdAsync(int id, User newEntity)
        {
            var existingUser = await _dbContext
                .Users
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.UserId == id);

            if (existingUser is null)
            {
                return null;
            }
            
            existingUser.FirstName = newEntity.FirstName;
            existingUser.LastName = newEntity.LastName;
            existingUser.Description = newEntity.Description;
            existingUser.PhoneNumber = newEntity.PhoneNumber;
            existingUser.ShouldHidePhoneNumber = newEntity.ShouldHidePhoneNumber;
            existingUser.Email = newEntity.Email;
            existingUser.IsVerified = newEntity.IsVerified;
            _dbContext.Users.Update(existingUser);
            
            await _dbContext.SaveChangesAsync();

            return existingUser;
        }

        public async Task<bool> CheckIfRoleExistsAsync(string roleName)
            => await _rolesRepository.GetByRoleNameAsync(roleName) is not null;

        public async Task<UserRole> GetRoleIdByNamesAsync(string roleName)
            => await _rolesRepository.GetByRoleNameAsync(roleName);

        public string GetUserRole(int userId)
        {
            return _dbContext.Users.Include(user => user.UserRole)
                .AsQueryable()
                .FirstOrDefault(user => user.UserId == userId)
                ?.UserRole?.UserRoleName;
        }
        
        public async Task<User> GetUserByTokenAsync(string token)
        {
            return await _dbContext.Users
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.ConfirmationToken == token);
        }
    }
}