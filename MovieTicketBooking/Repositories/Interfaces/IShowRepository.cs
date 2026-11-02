using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<IEnumerable<Show>> GetAllAsync();
        Task<Show?> GetByIdAsync(int showId);
        Task<Show?> GetByIdWithDetailsAsync(int showId);
        Task<IEnumerable<Show>> GetByMovieIdAsync(int movieId);
        Task<IEnumerable<Show>> GetByTheatreIdAsync(int theatreId);
        Task<IEnumerable<Show>> GetByDateAsync(DateTime date);
        Task<bool> HasOverlappingShowAsync(int screenId, DateTime showDate, TimeSpan startTime, TimeSpan endTime, int? excludeShowId = null);
        Task<Show> CreateAsync(Show show, IEnumerable<ShowSeat> showSeats);
        Task UpdateAsync(Show show);
        Task DeleteAsync(Show show);
        Task<bool> ExistsAsync(int showId);
    }
}
