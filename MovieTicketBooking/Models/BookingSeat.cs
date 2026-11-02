namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents the link between a booking and a specific seat in a show.
    /// Records the price at the time of booking.
    /// </summary>
    public class BookingSeat
    {
        public int BookingSeatId { get; set; }
        public int BookingId { get; set; }
        public int SeatId { get; set; }
        public int ShowSeatId { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public Booking Booking { get; set; } = null!;
        public Seat Seat { get; set; } = null!;
        public ShowSeat ShowSeat { get; set; } = null!;
    }
}
