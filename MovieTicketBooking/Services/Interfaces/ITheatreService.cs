using MovieTicketBooking.DTOs.Theatres;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<IEnumerable<TheatreResponseDto>> GetAllTheatresAsync();
        Task<TheatreResponseDto?> GetTheatreByIdAsync(int theatreId);
        Task<TheatreResponseDto> CreateTheatreAsync(CreateTheatreDto dto);
        Task<TheatreResponseDto> UpdateTheatreAsync(int theatreId, UpdateTheatreDto dto);
        Task DeleteTheatreAsync(int theatreId);
    }
}

