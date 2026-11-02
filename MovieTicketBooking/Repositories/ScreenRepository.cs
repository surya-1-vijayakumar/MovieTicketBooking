using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ScreenRepository> _logger;

        public ScreenRepository(ApplicationDbContext context, ILogger<ScreenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Screen>> GetAllAsync()
        {
            return await _context.Screens
                .AsNoTracking()
                .Include(s => s.Theatre)
                .OrderBy(s => s.Theatre.Name)
                .ThenBy(s => s.ScreenName)
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int screenId)
        {
            return await _context.Screens
                .AsNoTracking()
                .Include(s => s.Theatre)
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);
        }

        public async Task<IEnumerable<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Screens
                .AsNoTracking()
                .Include(s => s.Theatre)
                .Where(s => s.TheatreId == theatreId)
                .OrderBy(s => s.ScreenName)
                .ToListAsync();
        }

        public async Task<Screen> CreateAsync(Screen screen)
        {
            _context.Screens.Add(screen);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created screen with ID {ScreenId}", screen.ScreenId);
            return screen;
        }

        public async Task UpdateAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Screen screen)
        {
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int screenId)
        {
            return await _context.Screens.AnyAsync(s => s.ScreenId == screenId);
        }
    }
}
