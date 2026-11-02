using MovieTicketBooking.DTOs.Screens;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IScreenService
    {
        Task<IEnumerable<ScreenResponseDto>> GetAllScreensAsync();
        Task<ScreenResponseDto?> GetScreenByIdAsync(int screenId);
        Task<IEnumerable<ScreenResponseDto>> GetScreensByTheatreAsync(int theatreId);
        Task<ScreenResponseDto> CreateScreenAsync(CreateScreenDto dto);
        Task<ScreenResponseDto> UpdateScreenAsync(int screenId, UpdateScreenDto dto);
        Task DeleteScreenAsync(int screenId);
    }
}

