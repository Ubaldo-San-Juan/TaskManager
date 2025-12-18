using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TaskManager.Business.Interfaces;
using TaskManager.Business.Mappings;
using TaskManager.Business.Services;
using TaskManager.Business.Validators;
using TaskManager.Data.Context;
using TaskManager.Data.Interfaces;
using TaskManager.Data.Repositories;
namespace TaskManager.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            // Database Context
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();

            // Automapper
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            // Fluent validation
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

            // Business Services
            services.AddScoped<IUserService, UserService>();


            return services;
        }
    }
}
