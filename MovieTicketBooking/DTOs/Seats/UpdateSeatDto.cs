using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats
{
    public class UpdateSeatDto
    {
        [MaxLength(50)]
        public string SeatType { get; set; } = "Regular";

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public bool IsActive { get; set; }
    }
}
