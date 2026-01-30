using Microsoft.Extensions.DependencyInjection;
using MyBlog.Application.Interfaces;
using MyBlog.Infrastructure.Repositories;

namespace MyBlog.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            // Register all repositories
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            return services;
        }
    }
}
