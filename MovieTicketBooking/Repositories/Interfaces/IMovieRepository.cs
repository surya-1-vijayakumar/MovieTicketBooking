using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> GetAllAsync(int page, int pageSize);
        Task<int> GetTotalCountAsync();
        Task<Movie?> GetByIdAsync(int movieId);
        Task<IEnumerable<Movie>> SearchByTitleAsync(string title);
        Task<Movie> CreateAsync(Movie movie);
        Task UpdateAsync(Movie movie);
        Task DeleteAsync(Movie movie);
        Task<bool> ExistsAsync(int movieId);
    }
}
