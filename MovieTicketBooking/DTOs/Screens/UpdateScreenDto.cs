using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Screens
{
    public class UpdateScreenDto
    {
        [Required]
        [MaxLength(100)]
        public string ScreenName { get; set; } = string.Empty;

        [Required]
        [Range(1, 1000)]
        public int TotalSeats { get; set; }

        [MaxLength(50)]
        public string ScreenType { get; set; } = "Regular";

        public bool IsActive { get; set; }
    }
}
