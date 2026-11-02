using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Booking?> GetByIdAsync(int bookingId);
        Task<Booking?> GetByIdWithDetailsAsync(int bookingId);
        Task<IEnumerable<ShowSeat>> GetShowSeatsByIdsAsync(int showId, IEnumerable<int> seatIds);
        Task<Booking> CreateAsync(Booking booking, IEnumerable<BookingSeat> bookingSeats, IEnumerable<ShowSeat> showSeatsToUpdate);
        Task CancelAsync(Booking booking, IEnumerable<ShowSeat> showSeatsToRestore);
    }
}
