using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly ILogger<SeatService> _logger;

        public SeatService(ISeatRepository seatRepository, IScreenRepository screenRepository, ILogger<SeatService> logger)
        {
            _seatRepository = seatRepository;
            _screenRepository = screenRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<SeatResponseDto>> GetSeatsByScreenAsync(int screenId)
        {
            var screenExists = await _screenRepository.ExistsAsync(screenId);
            if (!screenExists)
                throw new KeyNotFoundException($"Screen with ID {screenId} was not found.");

            var seats = await _seatRepository.GetByScreenIdAsync(screenId);
            return seats.Select(MapToDto);
        }

        public async Task<SeatResponseDto?> GetSeatByIdAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            return seat is null ? null : MapToDto(seat);
        }

        public async Task<SeatResponseDto> CreateSeatAsync(CreateSeatDto dto)
        {
            var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);
            if (screen is null)
                throw new KeyNotFoundException($"Screen with ID {dto.ScreenId} was not found.");

            var rowLetterUpper = dto.RowLetter.ToUpper().Trim();
            var duplicateExists = await _seatRepository.ExistsByCoordinateAsync(dto.ScreenId, rowLetterUpper, dto.ColumnNumber);
            if (duplicateExists)
                throw new InvalidOperationException($"Seat {rowLetterUpper}{dto.ColumnNumber} already exists in screen {dto.ScreenId}.");

            var seat = new Seat
            {
                ScreenId = dto.ScreenId,
                RowLetter = rowLetterUpper,
                ColumnNumber = dto.ColumnNumber,
                SeatType = dto.SeatType.Trim(),
                Price = dto.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _seatRepository.CreateAsync(seat);
            var withDetails = await _seatRepository.GetByIdAsync(created.SeatId);
            return MapToDto(withDetails!);
        }

        public async Task<IEnumerable<SeatResponseDto>> CreateBulkSeatsAsync(BulkCreateSeatDto dto)
        {
            var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);
            if (screen is null)
                throw new KeyNotFoundException($"Screen with ID {dto.ScreenId} was not found.");

            var requestCoordinates = dto.Seats.Select(s => $"{s.RowLetter.ToUpper().Trim()}{s.ColumnNumber}").ToList();
            if (requestCoordinates.Count != requestCoordinates.Distinct().Count())
                throw new InvalidOperationException("Duplicate seat coordinates found within the request.");

            var conflictingSeats = new List<string>();
            foreach (var spec in dto.Seats)
            {
                var rowUpper = spec.RowLetter.ToUpper().Trim();
                var exists = await _seatRepository.ExistsByCoordinateAsync(dto.ScreenId, rowUpper, spec.ColumnNumber);
                if (exists)
                    conflictingSeats.Add($"{rowUpper}{spec.ColumnNumber}");
            }

            if (conflictingSeats.Any())
                throw new InvalidOperationException($"The following seat coordinates already exist: {string.Join(", ", conflictingSeats)}");

            var seats = dto.Seats.Select(spec => new Seat
            {
                ScreenId = dto.ScreenId,
                RowLetter = spec.RowLetter.ToUpper().Trim(),
                ColumnNumber = spec.ColumnNumber,
                SeatType = spec.SeatType.Trim(),
                Price = spec.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });

            var created = await _seatRepository.CreateBulkAsync(seats);
            var allSeats = await _seatRepository.GetByScreenIdAsync(dto.ScreenId);
            var createdIds = created.Select(s => s.SeatId).ToHashSet();
            return allSeats.Where(s => createdIds.Contains(s.SeatId)).Select(MapToDto);
        }

        public async Task<SeatResponseDto> UpdateSeatAsync(int seatId, UpdateSeatDto dto)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            if (seat is null)
                throw new KeyNotFoundException($"Seat with ID {seatId} was not found.");

            seat.SeatType = dto.SeatType.Trim();
            seat.Price = dto.Price;
            seat.IsActive = dto.IsActive;

            await _seatRepository.UpdateAsync(seat);
            var withDetails = await _seatRepository.GetByIdAsync(seatId);
            return MapToDto(withDetails!);
        }

        public async Task DeleteSeatAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            if (seat is null)
                throw new KeyNotFoundException($"Seat with ID {seatId} was not found.");

            await _seatRepository.DeleteAsync(seat);
        }

        private static SeatResponseDto MapToDto(Seat s) => new()
        {
            SeatId = s.SeatId,
            ScreenId = s.ScreenId,
            ScreenName = s.Screen?.ScreenName ?? string.Empty,
            RowLetter = s.RowLetter,
            ColumnNumber = s.ColumnNumber,
            SeatType = s.SeatType,
            Price = s.Price,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt
        };
    }
}

