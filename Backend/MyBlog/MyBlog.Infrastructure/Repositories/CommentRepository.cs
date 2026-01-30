using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBlog.Application.Interfaces;
using MyBlog.Domain.Entities;
using MyBlog.Infrastructure.Data;

namespace MyBlog.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Where(c => !c.IsDeleted)
                .Include(c => c.Blog)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Where(c => !c.IsDeleted)
                .Include(c => c.Blog)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Comment>> GetByBlogIdAsync(int blogId)
        {
            return await _context.Comments
                .Where(c => !c.IsDeleted && c.BlogId == blogId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Comment comment)
        {
            // Soft-delete: mark as deleted
            if (comment == null) return;

            comment.IsDeleted = true;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
