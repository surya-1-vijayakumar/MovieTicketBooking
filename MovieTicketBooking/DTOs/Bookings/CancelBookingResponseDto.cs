namespace MovieTicketBooking.DTOs.Bookings
{
    public class CancelBookingResponseDto
    {
        public int BookingId { get; set; }
        public string BookingStatus { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CancelledAt { get; set; }
    }
}
