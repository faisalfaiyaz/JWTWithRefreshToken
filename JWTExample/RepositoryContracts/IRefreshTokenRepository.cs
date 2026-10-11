using JWTExample.Entities;

namespace JWTExample.RepositoryContracts;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

    Task AddAsync(RefreshToken refreshToken);

    Task SaveChangesAsync();
}
