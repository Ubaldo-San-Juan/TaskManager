using TaskManager.Data.Context;
using TaskManager.Data.Seeders;
using Microsoft.Extensions.Hosting;

namespace TaskManager.API.Extensions
{
    public static class DataExtensions
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                logger.LogInformation("Seeding the database...");
                
                // Pass IsDevelopment flag to the seeder
                bool isDevelopment = app.Environment.IsDevelopment();
                await DbSeeder.SeedAsync(context, services, 10, 5, isDevelopment);
                
                logger.LogInformation("Database seeding completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
