using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetByScreenIdAsync(int screenId);
        Task<Seat?> GetByIdAsync(int seatId);
        Task<bool> ExistsByCoordinateAsync(int screenId, string rowLetter, int columnNumber, int? excludeSeatId = null);
        Task<Seat> CreateAsync(Seat seat);
        Task<IEnumerable<Seat>> CreateBulkAsync(IEnumerable<Seat> seats);
        Task UpdateAsync(Seat seat);
        Task DeleteAsync(Seat seat);
    }
}
