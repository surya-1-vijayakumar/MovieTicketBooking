using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IShowService
    {
        Task<IEnumerable<ShowResponseDto>> GetAllShowsAsync();
        Task<ShowResponseDto?> GetShowByIdAsync(int showId);
        Task<IEnumerable<ShowResponseDto>> GetShowsByMovieAsync(int movieId);
        Task<IEnumerable<ShowResponseDto>> GetShowsByTheatreAsync(int theatreId);
        Task<IEnumerable<ShowResponseDto>> GetShowsByDateAsync(DateTime date);
        Task<ShowResponseDto> CreateShowAsync(CreateShowDto dto);
        Task<ShowResponseDto> UpdateShowAsync(int showId, UpdateShowDto dto);
        Task DeleteShowAsync(int showId);
    }
}

