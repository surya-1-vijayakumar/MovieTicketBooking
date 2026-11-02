namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents a scheduled screening of a movie on a specific screen.
    /// </summary>
    public class Show
    {
        public int ShowId { get; set; }
        public int MovieId { get; set; }
        public int ScreenId { get; set; }
        public DateTime ShowDate { get; set; }      // Date of the show
        public TimeSpan StartTime { get; set; }     // e.g. 10:00:00
        public TimeSpan EndTime { get; set; }       // Computed: StartTime + DurationMinutes
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Movie Movie { get; set; } = null!;
        public Screen Screen { get; set; } = null!;
        public ICollection<ShowSeat> ShowSeats { get; set; } = new List<ShowSeat>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
