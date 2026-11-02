using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats
{
    public class CreateSeatDto
    {
        [Required]
        public int ScreenId { get; set; }

        [Required(ErrorMessage = "Row letter is required (e.g. A, B, C).")]
        [MaxLength(5)]
        public string RowLetter { get; set; } = string.Empty;

        [Required]
        [Range(1, 100, ErrorMessage = "Column number must be between 1 and 100.")]
        public int ColumnNumber { get; set; }

        [MaxLength(50)]
        public string SeatType { get; set; } = "Regular";

        [Required]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than zero.")]
        public decimal Price { get; set; }
    }
}
