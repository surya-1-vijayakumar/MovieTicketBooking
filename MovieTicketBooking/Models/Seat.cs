namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents a physical seat in a screen.
    /// RowLetter + ColumnNumber form a unique coordinate per screen (e.g. A1, B3).
    /// </summary>
    public class Seat
    {
        public int SeatId { get; set; }
        public int ScreenId { get; set; }
        public string RowLetter { get; set; } = string.Empty;  // e.g. A, B, C
        public int ColumnNumber { get; set; }                   // e.g. 1, 2, 3
        public string SeatType { get; set; } = "Regular";      // Regular, Premium, VIP
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Screen Screen { get; set; } = null!;
        public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
