using System;

namespace MyBlog.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string Comments { get; set; } = null!;
        public string CommentBy { get; set; } = null!;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;

        // Navigation property
        public virtual Blog? Blog { get; set; }
    }
}
