namespace GameStop.Api.Endpoints;

using GameStop.api.Dtos;
using GameStop.Api.Data;
using GameStop.Api.Dtos;
using GameStop.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class GameEndpoints
{
    const string GetGameEndpoint = "GetGame";
    private static readonly List<GameDto> games = new List<GameDto>
    {
        new GameDto
        {
            Id = 1,
            Title = "The Legend of Zelda: Breath of the Wild",
            Genre = "Action-Adventure",
            Price = 59.99m,
            ReleaseDate = new DateOnly(2017, 3, 3)
        },
        new GameDto
        {
            Id = 2, 
            Title = "Red Dead Redemption 2",
            Genre = "Action-Adventure",
            Price = 59.99m,
            ReleaseDate = new DateOnly(2018, 10, 26)
        },
        new GameDto
        {
            Id = 3,
            Title = "Cyberpunk 2077",
            Genre = "RPG",
            Price = 59.99m,
            ReleaseDate = new DateOnly(2020, 12, 10)
        }
    };

    public static void MapGameEndpoints(this WebApplication app)
    { 
         var group = app.MapGroup("/games").WithTags("games");
        // GET ENDPOINT //
        group.MapGet("/", async (GameStopContext dbContext) =>
        {
            var games = await dbContext.Games.Select(game => new GameDetailsDto(
                game.Id,
                game.Title,
                game.GenreId.ToString(),
                game.Price,
                game.ReleaseDate
            )).ToListAsync();

            return Results.Ok(games);
        });

        // GET BY ID ENDPOINT //
        group.MapGet("/{id}", async(int id, GameStopContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
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
        group.MapPost("/", async (CreateGameDto newGame, GameStopContext dbContext) =>
        {
            var game = new Game
            {
                Title = newGame.Title,
                GenreId = 1,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            dbContext.Games.Add(game);
            await dbContext.SaveChangesAsync();

            GameDetailsDto gameDto = new(
                game.Id,
                game.Title,
                game.GenreId.ToString(),
                game.Price,
                game.ReleaseDate
            );

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = gameDto.Id }, gameDto);
        });


        // PUT ENDPOINT //
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStopContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            game.Title = updateGame.Title;
            game.Price = updateGame.Price;
            game.ReleaseDate = updateGame.ReleaseDate;
            game.GenreId = 1;

            await dbContext.SaveChangesAsync();
            return Results.NoContent();
                
        });

        // DELETE ENDPOINT //
        group.MapDelete("/{id}", async(int id, GameStopContext dbContext) =>
        {
            var game = await dbContext.Games.FindAsync(id);
            if (game is null)
            {
                return Results.NotFound();
            }
            dbContext.Games.Remove(game);
            await dbContext.SaveChangesAsync();
            return Results.NoContent();
        });
    }
};