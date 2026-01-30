using System;

namespace MyBlog.Domain.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? FilePath { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int TotalView { get; set; } = 0;

        // Navigation property
        public virtual Category? Category { get; set; }
    }
}
