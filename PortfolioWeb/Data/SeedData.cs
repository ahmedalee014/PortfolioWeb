using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PortfolioWeb.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Ensure database is created and migrated
                await context.Database.EnsureCreatedAsync();
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migration completed");

                // Create roles first
                await CreateRoleAsync(roleManager, "Admin", logger);
                await CreateRoleAsync(roleManager, "Editor", logger);
                await CreateRoleAsync(roleManager, "Viewer", logger);

                // Create admin user with secure password
                await CreateAdminUserAsync(userManager, "admin@example.com", "Admin@1234", logger);

                // Optional: Create additional default users
                await CreateDefaultUserAsync(userManager, "editor@example.com", "Editor@1234", "Editor", logger);
                await CreateDefaultUserAsync(userManager, "viewer@example.com", "Viewer@1234", "Viewer", logger);

                logger.LogInformation("Seed data initialization completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database");
                throw;
            }
        }

        private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager,
            string roleName, ILogger<Program> logger)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation($"Creating {roleName} role...");
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));

                if (result.Succeeded)
                {
                    logger.LogInformation($"Successfully created {roleName} role");
                }
                else
                {
                    logger.LogError($"Failed to create {roleName} role. Errors: {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                logger.LogInformation($"{roleName} role already exists");
            }
        }

        private static async Task CreateAdminUserAsync(UserManager<IdentityUser> userManager,
            string email, string password, ILogger<Program> logger)
        {
            await CreateDefaultUserAsync(userManager, email, password, "Admin", logger);
        }

        private static async Task CreateDefaultUserAsync(UserManager<IdentityUser> userManager,
    string email, string password, string role, ILogger<Program> logger)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                logger.LogInformation($"Creating {role} user {email}...");

                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };

                var createResult = await userManager.CreateAsync(user, password);

                if (createResult.Succeeded)
                {
                    logger.LogInformation($"Successfully created {role} user {email}");

                    // Assign role
                    if (!string.IsNullOrEmpty(role))
                    {
                        var roleResult = await userManager.AddToRoleAsync(user, role);
                        if (roleResult.Succeeded)
                        {
                            logger.LogInformation($"Successfully assigned {role} role to {email}");
                        }
                        else
                        {
                            logger.LogError($"Failed to assign {role} role to {email}. Errors: {string.Join(", ", roleResult.Errors)}");
                        }
                    }
                }
                else
                {
                    logger.LogError($"Failed to create {role} user {email}. Errors: {string.Join(", ", createResult.Errors)}");

                    // Log password requirements if this is the failure
                    foreach (var error in createResult.Errors)
                    {
                        if (error.Code == "PasswordTooShort" ||
                            error.Code == "PasswordRequiresNonAlphanumeric" ||
                            error.Code == "PasswordRequiresDigit" ||
                            error.Code == "PasswordRequiresLower" ||
                            error.Code == "PasswordRequiresUpper")
                        {
                            logger.LogError($"Password requirement: {error.Description}");
                        }
                    }
                }
            }
            else
            {
                logger.LogInformation($"{role} user {email} already exists");
            }
        }
    }
}