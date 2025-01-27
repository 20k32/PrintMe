using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    internal class UserRepository
    {
        private readonly RolesRepository _rolesRepository;
        private readonly PrintMeDbContext _dbContext;

        public UserRepository(PrintMeDbContext dbContext, RolesRepository rolesRepository) 
            => (_dbContext, _rolesRepository) = (dbContext, rolesRepository);
        
        internal async Task AddUserAsync(User user)
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
    }
}