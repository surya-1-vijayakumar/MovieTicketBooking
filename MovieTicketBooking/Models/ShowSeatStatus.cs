namespace MovieTicketBooking.Models
{
    /// <summary>
    /// String constants for ShowSeat status values.
    /// Using constants prevents magic strings throughout the codebase.
    /// </summary>
    public static class ShowSeatStatus
    {
        public const string Available = "Available";
        public const string Booked = "Booked";
        public const string Blocked = "Blocked";
    }

    /// <summary>
    /// String constants for Booking status values.
    /// </summary>
    public static class BookingStatusConstants
    {
        public const string Confirmed = "Confirmed";
        public const string Cancelled = "Cancelled";
    }
}
