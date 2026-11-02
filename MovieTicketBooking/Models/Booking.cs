namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents a confirmed or cancelled ticket booking by a customer.
    /// Booking records are NEVER deleted — only their status changes.
    /// </summary>
    public class Booking
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; } = BookingStatusConstants.Confirmed;
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Show Show { get; set; } = null!;
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
