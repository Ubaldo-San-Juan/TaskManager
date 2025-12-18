using TaskManager.Data.Context;
using TaskManager.Data.Seeders;

namespace TaskManager.API.Extensions
{
    public static class DataExtensions
    {
        public static async Task SeedDataAsync(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment()) { return; }

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                logger.LogInformation("Seeding the database...");
                await DbSeeder.SeedAsync(context, 10, 5);
                logger.LogInformation("Database seeding completed.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
