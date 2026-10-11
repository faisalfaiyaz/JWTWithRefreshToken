using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace JWTExample.Entities;

[Index(nameof(TokenHash), IsUnique = true)]
public class RefreshToken
{
    [Key]
    public int RefreshTokenId { get; set; }

    public int UserId { get; set; }

    [Required]
    [MaxLength(64)]
    public string TokenHash { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime? RevokedAtUtc { get; set; }

    public User User { get; set; } = null!;
}
