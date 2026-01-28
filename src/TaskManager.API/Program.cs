using Microsoft.Extensions.DependencyInjection;
using TaskManager.API.Extensions;
using TaskManager.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
var app = builder.Build();

await app.SeedDataAsync();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Mi API v1");
    });
}

// app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
