namespace MovieTicketBooking.Models
{
    /// <summary>
    /// Represents a physical cinema theatre location.
    /// </summary>
    public class Theatre
    {
        public int TheatreId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property — one theatre has many screens
        public ICollection<Screen> Screens { get; set; } = new List<Screen>();
    }
}
