namespace GameStop.Api.Dtos;

public record GameDetailsDto
(
    int Id, 
    string Title, 
    string Genre, 
    decimal Price, 
    DateOnly ReleaseDate
);
