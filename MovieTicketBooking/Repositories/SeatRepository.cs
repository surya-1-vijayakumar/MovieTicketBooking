using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class SeatRepository : ISeatRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SeatRepository> _logger;

        public SeatRepository(ApplicationDbContext context, ILogger<SeatRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Seat>> GetByScreenIdAsync(int screenId)
        {
            return await _context.Seats
                .AsNoTracking()
                .Include(s => s.Screen)
                .Where(s => s.ScreenId == screenId)
                .OrderBy(s => s.RowLetter)
                .ThenBy(s => s.ColumnNumber)
                .ToListAsync();
        }

        public async Task<Seat?> GetByIdAsync(int seatId)
        {
            return await _context.Seats
                .AsNoTracking()
                .Include(s => s.Screen)
                .FirstOrDefaultAsync(s => s.SeatId == seatId);
        }

        public async Task<bool> ExistsByCoordinateAsync(int screenId, string rowLetter, int columnNumber, int? excludeSeatId = null)
        {
            var query = _context.Seats
                .Where(s => s.ScreenId == screenId
                         && s.RowLetter == rowLetter
                         && s.ColumnNumber == columnNumber);

            if (excludeSeatId.HasValue)
                query = query.Where(s => s.SeatId != excludeSeatId.Value);

            return await query.AnyAsync();
        }

        public async Task<Seat> CreateAsync(Seat seat)
        {
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created seat {RowLetter}{ColumnNumber} in screen {ScreenId}",
                seat.RowLetter, seat.ColumnNumber, seat.ScreenId);
            return seat;
        }

        public async Task<IEnumerable<Seat>> CreateBulkAsync(IEnumerable<Seat> seats)
        {
            var seatList = seats.ToList();
            await _context.Seats.AddRangeAsync(seatList);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Bulk created {Count} seats", seatList.Count);
            return seatList;
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Seats.Update(seat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Seat seat)
        {
            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();
        }
    }
}
