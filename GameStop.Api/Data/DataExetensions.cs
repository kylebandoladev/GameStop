using GameStop.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStop.Api.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GameStopContext>();
        try
        {
            db.Database.Migrate();
        }
        catch (Exception ex) // Exception handling for database migration errors
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    public static void AddGameStopDb(this WebApplicationBuilder builder)
    {
        var connString = builder.Configuration.GetConnectionString("GameStop"); // Use the connection string name from appsettings.json

        builder.Services.AddSqlite<GameStopContext>(
            connString,
        optionsAction: options => options.UseSeeding((context, _) =>
        {
            if (!context.Set<Genre>().Any())
            {
                context.Set<Genre>().AddRange
                (
                    new Genre { Name = "Action" },
                    new Genre { Name = "Adventure" },
                    new Genre { Name = "RPG" },
                    new Genre { Name = "Simulation" },
                    new Genre { Name = "Strategy" },
                    new Genre { Name = "Sports" }
                );

                context.SaveChanges();
            }
        }));
    }
}