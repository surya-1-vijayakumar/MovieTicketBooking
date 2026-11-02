using MovieTicketBooking.DTOs.Seats;

namespace MovieTicketBooking.Services.Interfaces
{
    public interface ISeatService
    {
        Task<IEnumerable<SeatResponseDto>> GetSeatsByScreenAsync(int screenId);
        Task<SeatResponseDto?> GetSeatByIdAsync(int seatId);
        Task<SeatResponseDto> CreateSeatAsync(CreateSeatDto dto);
        Task<IEnumerable<SeatResponseDto>> CreateBulkSeatsAsync(BulkCreateSeatDto dto);
        Task<SeatResponseDto> UpdateSeatAsync(int seatId, UpdateSeatDto dto);
        Task DeleteSeatAsync(int seatId);
    }
}

