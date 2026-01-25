using Microsoft.EntityFrameworkCore;

namespace MyBlog.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Define DbSets here, for example:
    // public DbSet<Post> Posts { get; set; }
}
