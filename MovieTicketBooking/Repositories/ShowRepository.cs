using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ShowRepository> _logger;

        public ShowRepository(ApplicationDbContext context, ILogger<ShowRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Show>> GetAllAsync()
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                .Include(s => s.ShowSeats)
                .OrderBy(s => s.ShowDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Show?> GetByIdAsync(int showId)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                .Include(s => s.ShowSeats)
                .FirstOrDefaultAsync(s => s.ShowId == showId);
        }

        public async Task<Show?> GetByIdWithDetailsAsync(int showId)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                .Include(s => s.ShowSeats)
                    .ThenInclude(ss => ss.Seat)
                .FirstOrDefaultAsync(s => s.ShowId == showId);
        }

        public async Task<IEnumerable<Show>> GetByMovieIdAsync(int movieId)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                .Include(s => s.ShowSeats)
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.ShowDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                .Include(s => s.ShowSeats)
                .Where(s => s.Screen.TheatreId == theatreId)
                .OrderBy(s => s.ShowDate)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Show>> GetByDateAsync(DateTime date)
        {
            return await _context.Shows
                .AsNoTracking()
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre)
                .Include(s => s.ShowSeats)
                .Where(s => s.ShowDate.Date == date.Date)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingShowAsync(int screenId, DateTime showDate, TimeSpan startTime, TimeSpan endTime, int? excludeShowId = null)
        {
            // Two shows overlap if: newStart < existingEnd AND newEnd > existingStart
            var query = _context.Shows
                .Where(s => s.ScreenId == screenId
                         && s.ShowDate.Date == showDate.Date
                         && s.StartTime < endTime
                         && s.EndTime > startTime);

            if (excludeShowId.HasValue)
                query = query.Where(s => s.ShowId != excludeShowId.Value);

            return await query.AnyAsync();
        }

        public async Task<Show> CreateAsync(Show show, IEnumerable<ShowSeat> showSeats)
        {
            // Use a transaction — show + all its ShowSeats must be created atomically.
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Shows.Add(show);
                await _context.SaveChangesAsync(); // Save show first to get ShowId

                // Auto-generate ShowSeat records for every physical seat in the screen
                foreach (var showSeat in showSeats)
                {
                    showSeat.ShowId = show.ShowId;
                }
                await _context.ShowSeats.AddRangeAsync(showSeats);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                _logger.LogInformation("Created show {ShowId} with {SeatCount} ShowSeats", show.ShowId, showSeats.Count());
                return show;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Show show)
        {
            _context.Shows.Remove(show);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int showId)
        {
            return await _context.Shows.AnyAsync(s => s.ShowId == showId);
        }
    }
}
