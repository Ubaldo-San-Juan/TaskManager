using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TaskManager.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, context, cancellationToken) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "TaskManager API",
                        Version = "v1",
                        Description = "API for TaskManager application"
                    };

                    // Define the Bearer Auth Scheme
                    var securityScheme = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Enter 'Bearer' [space] and then your token in the text input below.\nExample: 'Bearer 12345abcdef'",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    };

                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes.Add("Bearer", securityScheme);

                    // Apply the requirement globally
                    var securityRequirement = new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    };

                    document.SecurityRequirements.Add(securityRequirement);

                    return Task.CompletedTask;
                });
            });

            return services;
        }
    }
}