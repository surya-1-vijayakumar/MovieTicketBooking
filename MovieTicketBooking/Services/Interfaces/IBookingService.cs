using MovieTicketBooking.DTOs.Bookings;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto?> GetBookingByIdAsync(int bookingId);
        Task<BookingResponseDto> CreateBookingAsync(CreateBookingDto dto);
        Task<CancelBookingResponseDto> CancelBookingAsync(int bookingId);
    }
}

