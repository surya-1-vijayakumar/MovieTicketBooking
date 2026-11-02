using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.DTOs.Shows
{
    public class UpdateShowDto
    {
        [Required]
        public DateTime ShowDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        [Range(0.01, 10000.00)]
        public decimal BasePrice { get; set; }

        public bool IsActive { get; set; }
    }
}
