using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface ITheatreRepository
    {
        Task<IEnumerable<Theatre>> GetAllAsync();
        Task<Theatre?> GetByIdAsync(int theatreId);
        Task<Theatre> CreateAsync(Theatre theatre);
        Task UpdateAsync(Theatre theatre);
        Task DeleteAsync(Theatre theatre);
        Task<bool> ExistsAsync(int theatreId);
    }
}
