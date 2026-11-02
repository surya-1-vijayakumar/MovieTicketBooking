namespace MovieTicketBooking.DTOs.Seats
{
    public class SeatResponseDto
    {
        public int SeatId { get; set; }
        public int ScreenId { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public string RowLetter { get; set; } = string.Empty;
        public int ColumnNumber { get; set; }
        public string SeatNumber => $"{RowLetter}{ColumnNumber}"; // Computed: e.g. A1, B3
        public string SeatType { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
