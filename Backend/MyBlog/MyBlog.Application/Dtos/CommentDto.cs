using System;
using System.ComponentModel.DataAnnotations;

namespace MyBlog.Application.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }

        [Required]
        public int BlogId { get; set; }

        [Required]
        [StringLength(1000, MinimumLength = 2)]
        public string Comments { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string CommentBy { get; set; } = null!;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
    }
}
