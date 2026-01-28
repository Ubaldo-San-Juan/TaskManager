using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Data.Context;
using TaskManager.Data.Entities;

namespace TaskManager.Data.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context, IServiceProvider serviceProvider, int userCount, int tasksPerUsers, bool isDevelopment)
        {
            // Read admin credentials from appsettings
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var adminEmail = configuration["AdminSettings:Email"];
            var adminPassword = configuration["AdminSettings:Password"];

            // If not configuration found, show message
            if(string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                throw new Exception("Admin credentials not found in configuration. Please set AdminCredentials:Email and AdminCredentials:Password in appsettings.");
            }

            await context.Database.MigrateAsync();

            // Seed Roles
            var rolesToSeed = new List<Role>();
            
            if (!await context.Roles.AnyAsync(r => r.Name == "Admin"))
            {
                rolesToSeed.Add(new Role { Name = "Admin", Description = "Administrador con acceso total" });
            }

            if (!await context.Roles.AnyAsync(r => r.Name == "User"))
            {
                rolesToSeed.Add(new Role { Name = "User", Description = "Usuario estándar" });
            }

            if (rolesToSeed.Any())
            {
                await context.Roles.AddRangeAsync(rolesToSeed);
                await context.SaveChangesAsync();
            }

            // Seed Users
            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            var userRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "User");

            // Ensure Admin User Exists
            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Email == adminEmail);
            if (adminUser == null)
            {
                var myAdmin = new User
                {
                    Name = "Super Admin",
                    Email = adminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(adminPassword),
                    RoleId = adminRole!.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                
                await context.Users.AddAsync(myAdmin);
                await context.SaveChangesAsync();
            }

            // ONLY Generate Fake Data in Development
            if (isDevelopment)
            {
                // Verify if there are any users already present (for fake data)
                if (await context.Users.CountAsync() > 1) { return; } 

                var usersList = new List<User>();

                //Using bogus to generate Users
                var usersGenerator = new Faker<User>()
                    .RuleFor(u => u.Name, f => f.Name.FullName())
                    .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
                    .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20))
                    .RuleFor(u => u.RoleId, f => userRole!.Id)
                    .RuleFor(u => u.CreatedAt, f => f.Date.Past());

                var fakeUsers = usersGenerator.Generate(userCount);
                usersList.AddRange(fakeUsers);

                //Save users to get their Ids
                await context.Users.AddRangeAsync(usersList);
                await context.SaveChangesAsync();

                //Using bogus to generate Tasks
                var tasksGenerator = new Faker<TodoTask>()
                    .RuleFor(t => t.Title, f => f.Commerce.ProductName())
                    .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                    .RuleFor(t => t.IsCompleted, f => f.Random.Bool(0.3f))
                    .RuleFor(t => t.UserId, f => f.PickRandom(usersList).Id)
                    .RuleFor(t => t.CreatedAt, f => f.Date.Past());

                var tasks = tasksGenerator.Generate(userCount * tasksPerUsers);
                
                //Save tasks
                await context.Tasks.AddRangeAsync(tasks);
                await context.SaveChangesAsync();
            }
        }
    }
}
