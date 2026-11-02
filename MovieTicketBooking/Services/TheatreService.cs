using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;
        private readonly ILogger<TheatreService> _logger;

        public TheatreService(ITheatreRepository theatreRepository, ILogger<TheatreService> logger)
        {
            _theatreRepository = theatreRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<TheatreResponseDto>> GetAllTheatresAsync()
        {
            var theatres = await _theatreRepository.GetAllAsync();
            return theatres.Select(MapToDto);
        }

        public async Task<TheatreResponseDto?> GetTheatreByIdAsync(int theatreId)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);
            return theatre is null ? null : MapToDto(theatre);
        }

        public async Task<TheatreResponseDto> CreateTheatreAsync(CreateTheatreDto dto)
        {
            var theatre = new Theatre
            {
                Name = dto.Name.Trim(),
                Address = dto.Address.Trim(),
                City = dto.City.Trim(),
                State = dto.State?.Trim() ?? string.Empty,
                Phone = dto.Phone?.Trim(),
                Email = dto.Email?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _theatreRepository.CreateAsync(theatre);
            _logger.LogInformation("Theatre '{Name}' created with ID {TheatreId}", created.Name, created.TheatreId);
            return MapToDto(created);
        }

        public async Task<TheatreResponseDto> UpdateTheatreAsync(int theatreId, UpdateTheatreDto dto)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);
            if (theatre is null)
                throw new KeyNotFoundException($"Theatre with ID {theatreId} was not found.");

            theatre.Name = dto.Name.Trim();
            theatre.Address = dto.Address.Trim();
            theatre.City = dto.City.Trim();
            theatre.State = dto.State?.Trim() ?? string.Empty;
            theatre.Phone = dto.Phone?.Trim();
            theatre.Email = dto.Email?.Trim();
            theatre.IsActive = dto.IsActive;

            await _theatreRepository.UpdateAsync(theatre);
            return MapToDto(theatre);
        }

        public async Task DeleteTheatreAsync(int theatreId)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);
            if (theatre is null)
                throw new KeyNotFoundException($"Theatre with ID {theatreId} was not found.");

            await _theatreRepository.DeleteAsync(theatre);
        }

        private static TheatreResponseDto MapToDto(Theatre t) => new()
        {
            TheatreId = t.TheatreId,
            Name = t.Name,
            Address = t.Address,
            City = t.City,
            State = t.State,
            Phone = t.Phone,
            Email = t.Email,
            IsActive = t.IsActive,
            CreatedAt = t.CreatedAt
        };
    }
}

