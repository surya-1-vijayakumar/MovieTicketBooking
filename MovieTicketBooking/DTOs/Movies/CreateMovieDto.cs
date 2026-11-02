using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Movies
{
    public class CreateMovieDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        [MaxLength(100)]
        public string Genre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Language is required.")]
        [MaxLength(50)]
        public string Language { get; set; } = string.Empty;

        [Required]
        [Range(1, 600, ErrorMessage = "Duration must be between 1 and 600 minutes.")]
        public int DurationMinutes { get; set; }

        [Required(ErrorMessage = "Release date is required.")]
        public DateTime ReleaseDate { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "Rating must be between 0.0 and 10.0.")]
        public decimal Rating { get; set; }

        [MaxLength(500)]
        public string? PosterUrl { get; set; }
    }
}
