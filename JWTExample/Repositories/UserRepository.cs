using JWTExample.Context;
using JWTExample.Entities;
using JWTExample.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace JWTExample.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _db.Users
                            .AsNoTracking()
                            .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _db.Users.Add(user);

            await _db.SaveChangesAsync();

            return user;
        }
    }
}
