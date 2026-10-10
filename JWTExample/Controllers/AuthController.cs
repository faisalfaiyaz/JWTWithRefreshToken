using JWTExample.DTOs;
using JWTExample.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace JWTExample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        bool isRegistered = await _authService.RegisterAsync(request);

        if (!isRegistered)
        {
            return Conflict(new
                            {
                                message = "Email is already registered."
                            });
        }

        return Ok(new
                {
                    message = "User registered successfully."
                });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new
                                {
                                    message = "Invalid email or password."
                                });
        }

        return Ok(response);
    }
}
