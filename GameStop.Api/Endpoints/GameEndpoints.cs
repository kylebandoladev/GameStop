namespace GameStop.Api.Endpoints;

using GameStop.api.Dtos;
using GameStop.Api.Dtos;

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
        group.MapGet("/", () => games);

        // GET BY ID ENDPOINT //
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpoint);

        // POST ENDPOINT //
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            GameDto game = new GameDto
            {
                Id = games.Count + 1,
                Title = newGame.Title,
                Genre = newGame.Genre,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game);
        });


        // PUT ENDPOINT //
        group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
        {
            var game = games.Find(game => game.Id == id);
            if (game is null)
            {
                return Results.NotFound();
            }

            games[games.IndexOf(game)] = new GameDto
            {
                Id = game.Id,
                Title = updateGame.Title,
                Genre = updateGame.Genre,
                Price = updateGame.Price,
                ReleaseDate = updateGame.ReleaseDate
            };

            return Results.NoContent();
        });

        // DELETE ENDPOINT //
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            if (games.Exists(game => game.Id == id))
            {
                return Results.NotFound();
            }
            return Results.NoContent();
        });
    }
}