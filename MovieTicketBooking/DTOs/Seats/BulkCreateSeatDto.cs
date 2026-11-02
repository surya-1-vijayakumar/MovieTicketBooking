using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Seats
{
    public class BulkCreateSeatDto
    {
        [Required]
        public int ScreenId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "At least one seat specification is required.")]
        public List<SeatSpecDto> Seats { get; set; } = new();
    }

    public class SeatSpecDto
    {
        [Required]
        [MaxLength(5)]
        public string RowLetter { get; set; } = string.Empty;

        [Required]
        [Range(1, 100)]
        public int ColumnNumber { get; set; }

        [MaxLength(50)]
        public string SeatType { get; set; } = "Regular";

        [Required]
        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }
    }
}
