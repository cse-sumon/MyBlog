using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using MyBlog.Infrastructure.Identity;

namespace MyBlog.API.HostedServices
{
    /// <summary>
    /// Runs application data seeding at startup in a background task.
    /// This is idempotent and safe to run on every startup when enabled.
    /// </summary>
    public class StartupSeeder : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<StartupSeeder> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _env;

        public StartupSeeder(IServiceProvider services, ILogger<StartupSeeder> logger, IConfiguration configuration, IHostEnvironment env)
        {
            _services = services;
            _logger = logger;
            _configuration = configuration;
            _env = env;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Decide whether to seed: use config flag SeedOnStartup or default to Development environment
            var shouldSeed = _configuration.GetValue<bool?>("SeedOnStartup") ?? _env.IsDevelopment();
            if (!shouldSeed)
            {
                _logger.LogDebug("Startup seeding is disabled (SeedOnStartup=false and not Development).");
                return;
            }

            try
            {
                using var scope = _services.CreateScope();
                var services = scope.ServiceProvider;

                // Ensure database is created and migrations are applied before seeding
                try
                {
                    var db = services.GetRequiredService<MyBlog.Infrastructure.Data.AppDbContext>();
                    await db.Database.MigrateAsync(cancellationToken);
                    _logger.LogInformation("Database migrations applied successfully by StartupSeeder.");
                }
                catch (Exception migrateEx)
                {
                    _logger.LogError(migrateEx, "An error occurred while applying database migrations in StartupSeeder.");
                    // continue to attempt seeding; user manager operations will likely fail if DB not available
                }

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = new[] { "Admin", "User" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var r = await roleManager.CreateAsync(new IdentityRole(role));
                        if (!r.Succeeded)
                        {
                            _logger.LogWarning("Failed to create role {Role}: {Errors}", role, string.Join(',', r.Errors));
                        }
                    }
                }

                // Create default admin user (lookup by UserName since auth uses UserName)
                var adminUserName = _configuration.GetValue<string>("Admin:UserName") ?? "admin";
                var adminEmail = _configuration.GetValue<string>("Admin:Email") ?? "admin@gmail.com";
                var adminPassword = _configuration.GetValue<string>("Admin:Password") ?? "Admin@123";

                var adminUser = await userManager.FindByNameAsync(adminUserName);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser { UserName = adminUserName, Email = adminEmail };
                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                        _logger.LogInformation("Default admin user created: {UserName}", adminUserName);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create default admin user: {Errors}", string.Join(',', createResult.Errors));
                    }
                }
                else
                {
                    _logger.LogDebug("Admin user already exists: {UserName}", adminUserName);
                }

                // Create default regular user
                var userUserName = _configuration.GetValue<string>("User:UserName") ?? "user";
                var userEmail = _configuration.GetValue<string>("User:Email") ?? "user@gmail.com";
                var userPassword = _configuration.GetValue<string>("User:Password") ?? "User@123";

                var regularUser = await userManager.FindByNameAsync(userUserName);
                if (regularUser == null)
                {
                    regularUser = new ApplicationUser { UserName = userUserName, Email = userEmail };
                    var createUserResult = await userManager.CreateAsync(regularUser, userPassword);
                    if (createUserResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(regularUser, "User");
                        _logger.LogInformation("Default regular user created: {UserName}", userUserName);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to create default regular user: {Errors}", string.Join(',', createUserResult.Errors));
                    }
                }
                else
                {
                    _logger.LogDebug("Regular user already exists: {UserName}", userUserName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding roles and admin user.");
                // Do not rethrow; seeding should not prevent the app from starting in most cases.
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
