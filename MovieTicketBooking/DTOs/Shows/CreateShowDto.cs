using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Shows
{
    public class CreateShowDto
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        public int ScreenId { get; set; }

        [Required(ErrorMessage = "Show date is required.")]
        public DateTime ShowDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Base price must be greater than zero.")]
        public decimal BasePrice { get; set; }
    }
}
