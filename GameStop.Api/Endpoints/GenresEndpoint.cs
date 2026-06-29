namespace GameStop.Api.Endpoints;

using GameStop.Api.Data;
using GameStop.Api.Dtos;
using Microsoft.EntityFrameworkCore;

public static class GenresEndpoint
{
    public static void MapGenresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/genres").WithTags("genres");

        // GET ALL GENRES
        group.MapGet("/", async (GameStopContext dbContext, CancellationToken cancellationToken) =>
        {
            var genres = await dbContext.Genres
                .AsNoTracking()
                .Select(g => new GenreDto(g.Id, g.Name))
                .ToListAsync(cancellationToken);

            return Results.Ok(genres);
        });

        // GET GENRE BY ID
        group.MapGet("/{id}", async (int id, GameStopContext dbContext, CancellationToken cancellationToken) =>
        {
            var genre = await dbContext.Genres.FindAsync(new object[] { id }, cancellationToken: cancellationToken);
            return genre is null ? Results.NotFound() : Results.Ok(new GenreDto(genre.Id, genre.Name));
        });
    }
}