using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyBlog.Application.Dtos;
using MyBlog.Application.Interfaces;
using MyBlog.Domain.Entities;

namespace MyBlog.Application.Services
{
    public class BlogService
    {
        private readonly IBlogRepository _repo;
        private readonly FileService _fileService;

        public BlogService(IBlogRepository repo, FileService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }

        public async Task<List<BlogDto>> GetAllAsync()
        {
            var blogs = await _repo.GetAllAsync();
            return blogs.Select(b => MapBlogToDto(b)).ToList();
        }

        public async Task<BlogDto?> GetByIdAsync(int id)
        {
            var blog = await _repo.GetByIdAsync(id);
            if (blog == null) return null;

            // Increment view count
            await _repo.IncrementViewAsync(id);

            var dto = MapBlogToDto(blog);
            dto.TotalView++; // Increment for response
            return dto;
        }

        public async Task<BlogDto> CreateAsync(BlogDto dto)
        {
            var blog = new Blog
            {
                CategoryId = dto.CategoryId,
                Title = dto.Title,
                Description = dto.Description,
                FilePath = dto.FilePath,
                CreatedBy = dto.CreatedBy
            };

            await _repo.AddAsync(blog);
            return MapBlogToDto(blog);
        }

        public async Task UpdateAsync(int id, BlogDto dto)
        {
            var blog = await _repo.GetByIdAsync(id);
            if (blog == null) return;

            // Delete old file if new file is being uploaded
            if (!string.IsNullOrEmpty(dto.FilePath) && dto.FilePath != blog.FilePath)
            {
                await _fileService.DeleteFileAsync(blog.FilePath);
            }

            blog.CategoryId = dto.CategoryId;
            blog.Title = dto.Title;
            blog.Description = dto.Description;
            blog.FilePath = dto.FilePath;
            blog.IsActive = dto.IsActive;

            await _repo.UpdateAsync(blog);
        }

        public async Task DeleteAsync(int id)
        {
            var blog = await _repo.GetByIdAsync(id);
            if (blog == null) return;

            // Delete associated file
            if (!string.IsNullOrEmpty(blog.FilePath))
            {
                await _fileService.DeleteFileAsync(blog.FilePath);
            }

            await _repo.DeleteAsync(blog);
        }

        private static BlogDto MapBlogToDto(Blog blog)
        {
            return new BlogDto
            {
                Id = blog.Id,
                CategoryId = blog.CategoryId,
                Title = blog.Title,
                Description = blog.Description,
                FilePath = blog.FilePath,
                CreatedBy = blog.CreatedBy,
                CreatedDate = blog.CreatedDate,
                UpdatedDate = blog.UpdatedDate,
                IsActive = blog.IsActive,
                IsDeleted = blog.IsDeleted,
                TotalView = blog.TotalView,
                CategoryName = blog.Category?.Name
            };
        }
    }
}
