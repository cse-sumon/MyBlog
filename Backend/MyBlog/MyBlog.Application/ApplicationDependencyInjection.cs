using Microsoft.Extensions.DependencyInjection;
using MyBlog.Application.Services;

namespace MyBlog.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Register all application services here
            services.AddScoped<CategoryService>();
            services.AddScoped<BlogService>();
            services.AddScoped<CommentService>();
            services.AddScoped<FileService>();

            return services;
        }
    }
}
