using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Application.Dtos;
using MyBlog.Application.Interfaces;
using MyBlog.Domain.Entities;

namespace MyBlog.Application.Services
{
    public class CommentService
    {
        private readonly ICommentRepository _repo;

        public CommentService(ICommentRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<CommentDto>> GetAllAsync()
        {
            var comments = await _repo.GetAllAsync();
            return comments.Select(c => MapCommentToDto(c)).ToList();
        }

        public async Task<CommentDto?> GetByIdAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return null;
            return MapCommentToDto(comment);
        }

        public async Task<List<CommentDto>> GetByBlogIdAsync(int blogId)
        {
            var comments = await _repo.GetByBlogIdAsync(blogId);
            return comments.Select(c => MapCommentToDto(c)).ToList();
        }

        public async Task<CommentDto> CreateAsync(CommentDto dto)
        {
            var comment = new Comment
            {
                BlogId = dto.BlogId,
                Comments = dto.Comments,
                CommentBy = dto.CommentBy
            };

            await _repo.AddAsync(comment);
            return MapCommentToDto(comment);
        }

        public async Task UpdateAsync(int id, CommentDto dto)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return;

            comment.Comments = dto.Comments;
            comment.CommentBy = dto.CommentBy;

            await _repo.UpdateAsync(comment);
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return;
            await _repo.DeleteAsync(comment);
        }

        private static CommentDto MapCommentToDto(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                BlogId = comment.BlogId,
                Comments = comment.Comments,
                CommentBy = comment.CommentBy,
                CreatedDate = comment.CreatedDate,
                IsDeleted = comment.IsDeleted
            };
        }
    }
}
