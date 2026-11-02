using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IScreenRepository
    {
        Task<IEnumerable<Screen>> GetAllAsync();
        Task<Screen?> GetByIdAsync(int screenId);
        Task<IEnumerable<Screen>> GetByTheatreIdAsync(int theatreId);
        Task<Screen> CreateAsync(Screen screen);
        Task UpdateAsync(Screen screen);
        Task DeleteAsync(Screen screen);
        Task<bool> ExistsAsync(int screenId);
    }
}
