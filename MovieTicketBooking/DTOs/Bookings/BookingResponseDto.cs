namespace MovieTicketBooking.DTOs.Bookings
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ShowId { get; set; }
        public string MovieTitle { get; set; } = string.Empty;
        public string TheatreName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public DateTime ShowDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string? CustomerPhone { get; set; }
        public decimal TotalAmount { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public List<BookedSeatDto> BookedSeats { get; set; } = new();
    }

    public class BookedSeatDto
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; } = string.Empty; // e.g. A1
        public string SeatType { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}
