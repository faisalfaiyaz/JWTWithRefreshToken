
using System.ComponentModel.DataAnnotations;

namespace JWTExample.DTOs;

public class RefreshRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
