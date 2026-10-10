using JWTExample.Entities;

namespace JWTExample.ServiceContracts;

public interface IJwtService
{
    (string Token, DateTime ExpiresAt) GenerateToken(User user);
}
