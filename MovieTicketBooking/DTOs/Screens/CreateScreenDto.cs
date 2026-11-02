using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Screens
{
    public class CreateScreenDto
    {
        [Required]
        public int TheatreId { get; set; }

        [Required(ErrorMessage = "Screen name is required.")]
        [MaxLength(100)]
        public string ScreenName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000, ErrorMessage = "Total seats must be between 1 and 1000.")]
        public int TotalSeats { get; set; }

        [MaxLength(50)]
        public string ScreenType { get; set; } = "Regular";
    }
}
