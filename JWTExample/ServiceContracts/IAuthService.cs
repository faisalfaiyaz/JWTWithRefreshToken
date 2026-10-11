using JWTExample.DTOs;

namespace JWTExample.ServiceContracts;

public interface IAuthService
{
    Task<AuthenticationResponse?> LoginAsync(LoginRequest request);
    Task<bool> RegisterAsync(RegisterRequest request);
    //Task<AuthenticationResponse?> RefreshAsync(RefreshRequest request);
}