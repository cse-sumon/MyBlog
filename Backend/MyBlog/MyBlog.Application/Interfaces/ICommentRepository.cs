using System.Collections.Generic;
using System.Threading.Tasks;
using MyBlog.Domain.Entities;

namespace MyBlog.Application.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<List<Comment>> GetByBlogIdAsync(int blogId);
        Task AddAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task DeleteAsync(Comment comment);
    }
}
