using JWTExample.Context;
using JWTExample.Entities;
using JWTExample.RepositoryContracts;
using Microsoft.EntityFrameworkCore;

namespace JWTExample.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _db;

        public RefreshTokenRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return await _db.RefreshTokens
                            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);
        }

        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _db.RefreshTokens.AddAsync(refreshToken);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
