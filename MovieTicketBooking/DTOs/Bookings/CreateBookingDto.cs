using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Bookings
{
    public class CreateBookingDto
    {
        [Required]
        public int ShowId { get; set; }

        [Required(ErrorMessage = "At least one seat must be selected.")]
        [MinLength(1, ErrorMessage = "At least one seat ID is required.")]
        public List<int> SeatIds { get; set; } = new();

        [Required(ErrorMessage = "Customer name is required.")]
        [MaxLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Customer email is required.")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string CustomerEmail { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? CustomerPhone { get; set; }
    }
}
