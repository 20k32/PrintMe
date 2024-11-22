using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    internal class UserRepository
    {
        private readonly PrintMeDbContext _dbContext;

        public UserRepository(PrintMeDbContext dbContext) => _dbContext = dbContext;

        internal async Task AddUserAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
        internal Task<User> GetUserByEmailAsync(string email) =>
            _dbContext
                .Users
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.Email.Equals(email));

        internal async Task<User> UpdateUserByEmailAsync(string email, User newEntity)
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
        
        
        internal Task<User> GetUserByIdAsync(int id) =>
            _dbContext
                .Users
                .AsQueryable()
                .FirstOrDefaultAsync(user => user.UserId ==  id);

        internal async Task<User> UpdateUserByIdAsync(int id, User newEntity)
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
    }
}