using Api.Presentation.Common;
using Api.Presentation.Middleware;
using Api.Presentation.Setups;

using Core.Domain.Logger;
using Core.Infrastructure.Database.Seeds;
using Core.Infrastructure.Option;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;

using Scalar.AspNetCore;
using Core.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

#region Server Configuration
builder.Services.Configure<FormOptions>(options =>
{
    options.MemoryBufferThreshold = int.MaxValue; // Set max request body size to 100MB
    options.ValueCountLimit = int.MaxValue; // Set max request body size to 100MB
    options.MultipartBodyLengthLimit = int.MaxValue; // Set max request body size to 100MB
});
builder.WebHost.ConfigureKestrel((_, options) =>
{
    options.AddServerHeader = false; // Disable server header
    options.Limits.MaxRequestBodySize = 104857600; // Set max request body size to 100MB
});
builder.Services.Configure<IISServerOptions>(options => options.MaxRequestBodySize = 104857600); // Set max request body size to 100MB
#endregion

builder.Services.AddApiConfiguration(builder);
var app = builder.Build();

// Configure logger
var apiOption = builder.Configuration.GetSection(ApiOption.SectionName).Get<ApiOption>() ?? throw new NullReferenceException("Api Option could not be loaded from appsettings");
var staticLogger = app.Services.GetRequiredService<ILogger<StaticLogger>>();
StaticLogger.Logger = staticLogger;

/*// Seed database
using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<SeedInitializer>().ExecuteAsync();*/
// Apply EF Core migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        dbContext.Database.Migrate(); 
        await scope.ServiceProvider.GetRequiredService<SeedInitializer>().ExecuteAsync(); // <-- Seed
    }
    catch (Exception ex)
    {
        staticLogger.LogError(ex, "Failed to migrate or seed the database");
        throw;  
    }
}
// Configure middleware
app.UseExceptionHandler(opt => { });
app.UseMiddleware<RequestInformationMiddleware>();

// Configure OpenAPI
app.MapOpenApi(apiOption.OpenApi.OpenApiRoutePattern);
app.MapScalarApiReference(option => option
    .WithTitle(apiOption.OpenApi.Title)
    .WithEndpointPrefix(apiOption.OpenApi.ScalarEndpointRoutePattern)
    .WithPreferredScheme(JwtBearerDefaults.AuthenticationScheme)
    .WithOpenApiRoutePattern(apiOption.OpenApi.SubPath + apiOption.OpenApi.OpenApiRoutePattern)
);
app.MapGet("/", context =>
{
    context.Response.Redirect("/scalar/v1");
    return Task.CompletedTask;
});
// Configure authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure endpoints
foreach (var endpoint in app.Services.GetRequiredService<IEnumerable<IEndpoint>>())
    endpoint.MapRoutes(app);

app.Run();
