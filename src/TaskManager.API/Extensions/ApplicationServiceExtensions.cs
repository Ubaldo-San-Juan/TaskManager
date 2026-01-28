using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.Business.Common;
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
            services.AddScoped<IRoleRepository, RoleRepository>();

            // Automapper
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            // Fluent validation
            services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            // Auth Service
            services.AddScoped<IAuthService, AuthService>();
            // JWT Settings
            services.Configure<JwtSettings>(config.GetSection("JwtSettings"));


            // Configurar la autenticación con JWT
            var jwtSettings = config.GetSection("JwtSettings").Get<JwtSettings>();
            var key = Encoding.UTF8.GetBytes(jwtSettings!.SecretKey);

            // 2. Activamos el servicio de Autenticación
            services.AddAuthentication(options =>
            {
                // Definimos que por defecto usaremos JWT Bearer
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Validar que la clave de firma sea correcta
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    // Validar el Emisor (Issuer)
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,

                    // Validar la Audiencia (Audience)
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,

                    // Validar que no haya expirado
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Sin tiempo de gracia
                };
            });

            return services;
        }
    }
}
