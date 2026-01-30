using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Application.Dtos
{
    public class BlogDto
    {
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Title { get; set; } = null!;

        [Required]
        [StringLength(5000, MinimumLength = 10)]
        public string Description { get; set; } = null!;

        public string? FilePath { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int TotalView { get; set; } = 0;
        public string? CategoryName { get; set; }
    }
}
