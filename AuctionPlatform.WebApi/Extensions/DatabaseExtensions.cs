using AuctionPlatform.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.WebApi.Extensions;

public static class DatabaseExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AuctionDbContext>();
        context.Database.Migrate();
    }
}