namespace GameStop.api.Dtos;

public record class GameDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public required string Genre { get; init; }
    public decimal Price { get; init; }
    public DateOnly ReleaseDate { get; init;}

};