using System.ComponentModel.DataAnnotations;

namespace MyBlog.Application.Dtos
{
    public class CreateCategoryDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
