using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class TheatreRepository : ITheatreRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TheatreRepository> _logger;

        public TheatreRepository(ApplicationDbContext context, ILogger<TheatreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Theatre>> GetAllAsync()
        {
            return await _context.Theatres
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<Theatre?> GetByIdAsync(int theatreId)
        {
            return await _context.Theatres
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TheatreId == theatreId);
        }

        public async Task<Theatre> CreateAsync(Theatre theatre)
        {
            _context.Theatres.Add(theatre);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created theatre with ID {TheatreId}", theatre.TheatreId);
            return theatre;
        }

        public async Task UpdateAsync(Theatre theatre)
        {
            _context.Theatres.Update(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Theatre theatre)
        {
            _context.Theatres.Remove(theatre);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int theatreId)
        {
            return await _context.Theatres.AnyAsync(t => t.TheatreId == theatreId);
        }
    }
}
