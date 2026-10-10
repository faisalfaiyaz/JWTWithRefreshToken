using JWTExample.Entities;
using Microsoft.EntityFrameworkCore;

namespace JWTExample.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}
