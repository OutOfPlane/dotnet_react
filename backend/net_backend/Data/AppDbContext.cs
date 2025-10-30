using Microsoft.EntityFrameworkCore;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Models.Product> Products { get; set; }
    public DbSet<Models.User> Users { get; set; }
}