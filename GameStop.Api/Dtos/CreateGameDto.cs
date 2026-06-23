using System.ComponentModel.DataAnnotations;

namespace GameStop.Api.Dtos 
{
    public class CreateGameDto
    {
         
        [Required(ErrorMessage = "Title is required")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 50 characters")]
        public required string Title { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        [StringLength(30, MinimumLength = 1, ErrorMessage = "Genre must be between 1 and 30 characters")]
        public required string Genre { get; set; }
        [Range(0, 100, ErrorMessage = "Price must be a below 100")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Release date is required")]
        public DateOnly ReleaseDate { get; set; }
    }
} 


