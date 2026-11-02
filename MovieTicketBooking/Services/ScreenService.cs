using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;
        private readonly ITheatreRepository _theatreRepository;
        private readonly ILogger<ScreenService> _logger;

        public ScreenService(IScreenRepository screenRepository, ITheatreRepository theatreRepository, ILogger<ScreenService> logger)
        {
            _screenRepository = screenRepository;
            _theatreRepository = theatreRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ScreenResponseDto>> GetAllScreensAsync()
        {
            var screens = await _screenRepository.GetAllAsync();
            return screens.Select(MapToDto);
        }

        public async Task<ScreenResponseDto?> GetScreenByIdAsync(int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);
            return screen is null ? null : MapToDto(screen);
        }

        public async Task<IEnumerable<ScreenResponseDto>> GetScreensByTheatreAsync(int theatreId)
        {
            var theatreExists = await _theatreRepository.ExistsAsync(theatreId);
            if (!theatreExists)
                throw new KeyNotFoundException($"Theatre with ID {theatreId} was not found.");

            var screens = await _screenRepository.GetByTheatreIdAsync(theatreId);
            return screens.Select(MapToDto);
        }

        public async Task<ScreenResponseDto> CreateScreenAsync(CreateScreenDto dto)
        {
            var theatreExists = await _theatreRepository.ExistsAsync(dto.TheatreId);
            if (!theatreExists)
                throw new KeyNotFoundException($"Theatre with ID {dto.TheatreId} was not found.");

            var screen = new Screen
            {
                TheatreId = dto.TheatreId,
                ScreenName = dto.ScreenName.Trim(),
                TotalSeats = dto.TotalSeats,
                ScreenType = dto.ScreenType.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _screenRepository.CreateAsync(screen);
            var withDetails = await _screenRepository.GetByIdAsync(created.ScreenId);
            return MapToDto(withDetails!);
        }

        public async Task<ScreenResponseDto> UpdateScreenAsync(int screenId, UpdateScreenDto dto)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);
            if (screen is null)
                throw new KeyNotFoundException($"Screen with ID {screenId} was not found.");

            screen.ScreenName = dto.ScreenName.Trim();
            screen.TotalSeats = dto.TotalSeats;
            screen.ScreenType = dto.ScreenType.Trim();
            screen.IsActive = dto.IsActive;

            await _screenRepository.UpdateAsync(screen);
            var withDetails = await _screenRepository.GetByIdAsync(screenId);
            return MapToDto(withDetails!);
        }

        public async Task DeleteScreenAsync(int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);
            if (screen is null)
                throw new KeyNotFoundException($"Screen with ID {screenId} was not found.");

            await _screenRepository.DeleteAsync(screen);
        }

        private static ScreenResponseDto MapToDto(Screen s) => new()
        {
            ScreenId = s.ScreenId,
            TheatreId = s.TheatreId,
            TheatreName = s.Theatre?.Name ?? string.Empty,
            ScreenName = s.ScreenName,
            TotalSeats = s.TotalSeats,
            ScreenType = s.ScreenType,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt
        };
    }
}

