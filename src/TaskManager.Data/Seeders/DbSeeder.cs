using Bogus;
using Microsoft.EntityFrameworkCore;
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
        public static async Task SeedAsync(ApplicationDbContext context, int userCount, int tasksPerUsers)
        {
            await context.Database.MigrateAsync();

            // Verify if there are any users already present
            if (await context.Users.AnyAsync()) { return; }

            //Using bogus to generate Users
            var usersGenerator = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20));

            var users = usersGenerator.Generate(userCount);

            //Save users to get their Ids
            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            //Using bogus to generate Tasks
            var tasksGenerator = new Faker<TodoTask>()
                .RuleFor(t => t.Title, f => f.Commerce.ProductName())
                .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                .RuleFor(t => t.IsCompleted, f => f.Random.Bool(0.3f))
                .RuleFor(t => t.UserId, f => f.PickRandom(users).Id)
                .RuleFor(t => t.CreatedAt, f => f.Date.Past());

            var tasks = tasksGenerator.Generate(userCount * tasksPerUsers);
            
            //Save tasks
            await context.Tasks.AddRangeAsync(tasks);
            await context.SaveChangesAsync();
        }
    }
}
