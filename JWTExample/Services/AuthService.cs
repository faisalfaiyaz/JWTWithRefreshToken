using JWTExample.DTOs;
using JWTExample.Entities;
using JWTExample.RepositoryContracts;
using JWTExample.ServiceContracts;

namespace JWTExample.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthService(IUserRepository userRepository,IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthenticationResponse?> LoginAsync(LoginRequest request)
    {
        // 1. Find user by email
        var user = await _userRepository.GetUserByEmailAsync(request.Email);

        if (user == null)
        {
            return null;
        }

        // 2. Verify password using BCrypt
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password,user.PasswordHash);

        if (!isPasswordValid)
        {
            return null;
        }

        // 3. Generate JWT
        var (token, expiresAt) = _jwtService.GenerateToken(user);

        // 4. Return authentication response
        AuthenticationResponse authenticationResponse = new ()
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,
            Token = token,
            ExpiresAt = expiresAt
        };

        return authenticationResponse;
    }


    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        var email = request.Email.Trim();

        if (await _userRepository.EmailExistsAsync(email))
        {
            return false;
        }

        User user = new ()
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepository.CreateUserAsync(user);

        return true;
    }

}