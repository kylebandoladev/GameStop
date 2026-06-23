namespace GameStop.Api.Models;

public class Game
{
    public int Id {get; set;}
    public required string Title { get; set;}
    public Genre? Genre {get; set;}
    public int GenreId {get; set;}
} 