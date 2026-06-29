namespace GameStop.Api.Endpoints;

using GameStop.Api.Data;
using GameStop.Api.Dtos;
using GameStop.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class GameEndpoints
{
    const string GetGameEndpoint = "GetGame";
    public static void MapGameEndpoints(this WebApplication app)
    { 
         var group = app.MapGroup("/games").WithTags("games");
        // GET ENDPOINT //
        group.MapGet("/", async (GameStopContext dbContext, CancellationToken cancellationToken) =>
        {
            var games = await dbContext.Games.AsNoTracking().Select(game => new GameDetailsDto(
                game.Id,
                game.Title,
                game.GenreId.ToString(),
                game.Price,
                game.ReleaseDate
            )).ToListAsync(cancellationToken);

            return Results.Ok(games);
        });

        // GET BY ID ENDPOINT //
        group.MapGet("/{id}", async(int id, GameStopContext dbContext, CancellationToken cancellationToken) =>
        {
            var game = await dbContext.Games.FindAsync(id, cancellationToken);
            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Title,
                    game.GenreId.ToString(),
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
            .WithName(GetGameEndpoint);

        // POST ENDPOINT //
        group.MapPost("/", async (CreateGameDto newGame, GameStopContext dbContext, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("GameEndpoints");

            try
            {
                var game = new Game
                {
                    Title = newGame.Title,
                    GenreId = 1,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate
                };
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync(cancellationToken);

                logger.LogInformation("Created game {GameId} with title {GameTitle}", game.Id, game.Title);

                GameDetailsDto gameDto = new(
                    game.Id,
                    game.Title,
                    game.GenreId.ToString(),
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpoint, new { id = gameDto.Id }, gameDto);
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Database update error while creating a game");
                return Results.Problem("A database error occurred while creating the game.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while creating a game");
                return Results.Problem("An unexpected error occurred while creating the game.");
            }
        });


        // PUT ENDPOINT //
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStopContext dbContext, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("GameEndpoints");

            try
            {
                var game = await dbContext.Games.FindAsync(id, cancellationToken);
                if (game is null)
                {
                    logger.LogWarning("Game {GameId} was not found for update", id);
                    return Results.NotFound();
                }

                game.Title = updateGame.Title;
                game.Price = updateGame.Price;
                game.ReleaseDate = updateGame.ReleaseDate;
                game.GenreId = 1;

                await dbContext.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Updated game {GameId}", id);
                return Results.NoContent();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Database update error while updating game {GameId}", id);
                return Results.Problem("A database error occurred while updating the game.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while updating game {GameId}", id);
                return Results.Problem("An unexpected error occurred while updating the game.");
            }
        });

        // DELETE ENDPOINT //
        group.MapDelete("/{id}", async(int id, GameStopContext dbContext, ILoggerFactory loggerFactory, CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("GameEndpoints");

            try
            {
                var game = await dbContext.Games.FindAsync(id, cancellationToken);
                if (game is null)
                {
                    logger.LogWarning("Game {GameId} was not found for delete", id);
                    return Results.NotFound();
                }

                dbContext.Games.Remove(game);
                await dbContext.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Deleted game {GameId}", id);
                return Results.NoContent();
            }
            catch (DbUpdateException ex)
            {
                logger.LogError(ex, "Database update error while deleting game {GameId}", id);
                return Results.Problem("A database error occurred while deleting the game.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unexpected error while deleting game {GameId}", id);
                return Results.Problem("An unexpected error occurred while deleting the game.");
            }
        });
    }
};