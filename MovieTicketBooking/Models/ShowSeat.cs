namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents the availability status of each seat for a specific show.
    /// Auto-generated when a show is created. Status: Available → Booked → Available (on cancellation).
    /// </summary>
    public class ShowSeat
    {
        public int ShowSeatId { get; set; }
        public int ShowId { get; set; }
        public int SeatId { get; set; }
        public string Status { get; set; } = ShowSeatStatus.Available;
        public decimal Price { get; set; }

        // Navigation properties
        public Show Show { get; set; } = null!;
        public Seat Seat { get; set; } = null!;
        public ICollection<BookingSeat> BookingSeats { get; set; } = new List<BookingSeat>();
    }
}
