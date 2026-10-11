
using JWTExample.DTOs;
using JWTExample.Entities;
using JWTExample.RepositoryContracts;
using JWTExample.ServiceContracts;

namespace JWTExample.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService,
            IRefreshTokenRepository refreshTokenRepository,
            IRefreshTokenService refreshTokenService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _refreshTokenRepository = refreshTokenRepository;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<AuthenticationResponse?> LoginAsync(
            LoginRequest request)
        {
            var user = await _userRepository
                .GetUserByEmailAsync(request.Email.Trim());

            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

            if (!isPasswordValid)
            {
                return null;
            }

            // Generate Access Token
            var (accessToken, accessTokenExpiresAt) = _jwtService.GenerateToken(user);

            // Generate Refresh Token
            var refreshToken =
                _refreshTokenService.GenerateRefreshToken();

            var refreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);

            // Store only the hash in the database
            RefreshToken refreshTokenEntity = new RefreshToken
            {
                UserId = user.UserId,
                TokenHash = _refreshTokenService.HashRefreshToken(refreshToken),
                CreatedAtUtc = DateTime.UtcNow,
                ExpiresAtUtc = DateTime.UtcNow.AddDays(7)
            };

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            await _refreshTokenRepository.SaveChangesAsync();

            return new AuthenticationResponse
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Token = accessToken,
                ExpiresAt = accessTokenExpiresAt,
                RefreshToken = refreshToken,
                RefreshTokenExpiresAt = refreshTokenExpiresAt
            };
        }

        public async Task<bool> RegisterAsync(
            RegisterRequest request)
        {
            var email = request.Email.Trim();

            if (await _userRepository.EmailExistsAsync(email))
            {
                return false;
            }

            var user = new User
            {
                Name = request.Name.Trim(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(
                    request.Password)
            };

            await _userRepository.CreateUserAsync(user);

            return true;
        }
    }
}
