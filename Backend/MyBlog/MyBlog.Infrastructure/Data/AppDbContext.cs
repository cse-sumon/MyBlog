using Microsoft.EntityFrameworkCore;
using MyBlog.Domain.Entities;

namespace MyBlog.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Define DbSets here
    public DbSet<Category> Categories { get; set; } = null!;
}
