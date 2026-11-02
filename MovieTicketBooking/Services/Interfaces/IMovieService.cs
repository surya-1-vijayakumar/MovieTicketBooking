using MovieTicketBooking.DTOs.Movies;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IMovieService
    {
        Task<PagedMovieResponseDto> GetAllMoviesAsync(int page, int pageSize);
        Task<MovieResponseDto?> GetMovieByIdAsync(int movieId);
        Task<IEnumerable<MovieSummaryDto>> SearchMoviesByTitleAsync(string title);
        Task<MovieResponseDto> CreateMovieAsync(CreateMovieDto dto);
        Task<MovieResponseDto> UpdateMovieAsync(int movieId, UpdateMovieDto dto);
        Task DeleteMovieAsync(int movieId);
    }
}

