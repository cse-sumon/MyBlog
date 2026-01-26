using Microsoft.AspNetCore.Identity;

namespace MyBlog.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Extend with additional profile properties if needed
        public string? FullName { get; set; }
    }
}
