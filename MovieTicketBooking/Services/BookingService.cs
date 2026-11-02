using MovieTicketBooking.DTOs.Bookings;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowRepository _showRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IBookingRepository bookingRepository,
            IShowRepository showRepository,
            ILogger<BookingService> logger)
        {
            _bookingRepository = bookingRepository;
            _showRepository = showRepository;
            _logger = logger;
        }

        public async Task<BookingResponseDto?> GetBookingByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId);
            return booking is null ? null : MapToDto(booking);
        }

        public async Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto)
        {
            var show = await _showRepository.GetByIdWithDetailsAsync(dto.ShowId);
            if (show is null)
                throw new KeyNotFoundException($"Show with ID {dto.ShowId} was not found.");

            if (dto.SeatIds.Count != dto.SeatIds.Distinct().Count())
                throw new InvalidOperationException("Duplicate seat IDs found in the request.");

            var showSeats = (await _bookingRepository.GetShowSeatsByIdsAsync(dto.ShowId, dto.SeatIds)).ToList();

            if (showSeats.Count != dto.SeatIds.Count)
            {
                var foundSeatIds = showSeats.Select(ss => ss.SeatId).ToHashSet();
                var missingSeatIds = dto.SeatIds.Where(id => !foundSeatIds.Contains(id));
                throw new InvalidOperationException(
                    $"The following seat IDs do not belong to show {dto.ShowId}: {string.Join(", ", missingSeatIds)}");
            }

            var unavailableSeats = showSeats.Where(ss => ss.Status != ShowSeatStatus.Available).ToList();
            if (unavailableSeats.Any())
            {
                var unavailableSeatIds = unavailableSeats.Select(ss => ss.SeatId);
                throw new InvalidOperationException(
                    $"The following seats are not available: {string.Join(", ", unavailableSeatIds)}");
            }

            var totalAmount = showSeats.Sum(ss => ss.Price);

            var booking = new Booking
            {
                ShowId = dto.ShowId,
                CustomerName = dto.CustomerName.Trim(),
                CustomerEmail = dto.CustomerEmail.Trim().ToLower(),
                CustomerPhone = dto.CustomerPhone?.Trim(),
                TotalAmount = totalAmount,
                BookingStatus = BookingStatusConstants.Confirmed,
                BookingDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            var bookingSeats = showSeats.Select(ss => new BookingSeat
            {
                SeatId = ss.SeatId,
                ShowSeatId = ss.ShowSeatId,
                Price = ss.Price
            }).ToList();

            var created = await _bookingRepository.CreateAsync(booking, bookingSeats, showSeats);

            _logger.LogInformation("Booking {BookingId} created for customer '{Email}', amount: {Amount}",
                created.BookingId, created.CustomerEmail, totalAmount);

            var withDetails = await _bookingRepository.GetByIdWithDetailsAsync(created.BookingId);
            return MapToDto(withDetails!);
        }

        public async Task<CancelBookingResponseDto> CancelBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdWithDetailsAsync(bookingId);
            if (booking is null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} was not found.");

            if (booking.BookingStatus != BookingStatusConstants.Confirmed)
                throw new InvalidOperationException(
                    $"Booking {bookingId} cannot be cancelled because its status is '{booking.BookingStatus}'.");

            var showSeatsToRestore = (await _bookingRepository.GetShowSeatsByIdsAsync(
                booking.ShowId, booking.BookingSeats.Select(bs => bs.SeatId))).ToList();

            await _bookingRepository.CancelAsync(booking, showSeatsToRestore);

            _logger.LogInformation("Booking {BookingId} cancelled — {SeatCount} seat(s) restored to Available",
                bookingId, showSeatsToRestore.Count);

            return new CancelBookingResponseDto
            {
                BookingId = bookingId,
                BookingStatus = BookingStatusConstants.Cancelled,
                Message = $"Booking {bookingId} has been successfully cancelled. {showSeatsToRestore.Count} seat(s) are now available again.",
                CancelledAt = DateTime.UtcNow
            };
        }

        private static BookingResponseDto MapToDto(Booking b) => new()
        {
            BookingId = b.BookingId,
            ShowId = b.ShowId,
            MovieTitle = b.Show?.Movie?.Title ?? string.Empty,
            TheatreName = b.Show?.Screen?.Theatre?.Name ?? string.Empty,
            ScreenName = b.Show?.Screen?.ScreenName ?? string.Empty,
            ShowDate = b.Show?.ShowDate ?? default,
            StartTime = b.Show?.StartTime ?? default,
            CustomerName = b.CustomerName,
            CustomerEmail = b.CustomerEmail,
            CustomerPhone = b.CustomerPhone,
            TotalAmount = b.TotalAmount,
            BookingStatus = b.BookingStatus,
            BookingDate = b.BookingDate,
            BookedSeats = b.BookingSeats?.Select(bs => new BookedSeatDto
            {
                SeatId = bs.SeatId,
                SeatNumber = $"{bs.Seat?.RowLetter}{bs.Seat?.ColumnNumber}",
                SeatType = bs.Seat?.SeatType ?? string.Empty,
                Price = bs.Price
            }).ToList() ?? new()
        };
    }
}

