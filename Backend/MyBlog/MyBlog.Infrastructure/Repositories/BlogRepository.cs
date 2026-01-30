using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBlog.Application.Interfaces;
using MyBlog.Domain.Entities;
using MyBlog.Infrastructure.Data;

namespace MyBlog.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Blog>> GetAllAsync()
        {
            return await _context.Blogs
                .Where(b => !b.IsDeleted && b.IsActive)
                .Include(b => b.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Blog?> GetByIdAsync(int id)
        {
            return await _context.Blogs
                .Where(b => !b.IsDeleted && b.IsActive)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Blog blog)
        {
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Blog blog)
        {
            blog.UpdatedDate = System.DateTime.UtcNow;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Blog blog)
        {
            // Soft-delete: mark as deleted
            if (blog == null) return;

            blog.IsDeleted = true;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
        }

        public async Task IncrementViewAsync(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null && !blog.IsDeleted)
            {
                blog.TotalView++;
                _context.Blogs.Update(blog);
                await _context.SaveChangesAsync();
            }
        }
    }
}
