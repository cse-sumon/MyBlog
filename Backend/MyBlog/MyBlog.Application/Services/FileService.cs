using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MyBlog.Application.Services
{
    public class FileService
    {
        private readonly string _uploadsFolder;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public FileService()
        {
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "blogs");
            
            // Create uploads directory if it doesn't exist
            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public async Task<string?> UploadFileAsync(object fileObj)
        {
            if (fileObj is not IFormFile file || file.Length == 0)
                return null;

            // Validate file size
            if (file.Length > MaxFileSize)
                throw new InvalidOperationException("File size exceeds 5 MB limit.");

            // Validate file extension
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if (Array.IndexOf(_allowedExtensions, fileExtension) < 0)
                throw new InvalidOperationException("Only jpg, jpeg, and png files are allowed.");

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_uploadsFolder, uniqueFileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for database storage
            return $"/uploads/blogs/{uniqueFileName}";
        }

        public async Task DeleteFileAsync(string? filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            try
            {
                // Convert relative path to absolute path
                var absolutePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
                
                if (File.Exists(absolutePath))
                {
                    File.Delete(absolutePath);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't throw - file deletion shouldn't break the operation
                System.Diagnostics.Debug.WriteLine($"Error deleting file: {ex.Message}");
            }

            await Task.CompletedTask;
        }
    }
}
