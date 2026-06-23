namespace GameStop.Api.Dtos;

public record class UpdateGameDto
{
    public required string Title { get; init; }
    public required string Genre { get; init; }
    public decimal Price { get; init; }
    public DateOnly ReleaseDate { get; init;}
}