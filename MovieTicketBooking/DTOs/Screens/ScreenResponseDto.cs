namespace MovieTicketBooking.DTOs.Screens
{
    public class ScreenResponseDto
    {
        public int ScreenId { get; set; }
        public int TheatreId { get; set; }
        public string TheatreName { get; set; } = string.Empty;
        public string ScreenName { get; set; } = string.Empty;
        public int TotalSeats { get; set; }
        public string ScreenType { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
