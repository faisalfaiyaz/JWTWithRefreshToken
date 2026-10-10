using JWTExample.Entities;

namespace JWTExample.RepositoryContracts;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);

    Task<User> CreateUserAsync(User user);
}