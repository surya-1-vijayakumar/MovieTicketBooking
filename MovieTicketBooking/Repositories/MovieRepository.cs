using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;

namespace MovieTicketBooking.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MovieRepository> _logger;

        public MovieRepository(ApplicationDbContext context, ILogger<MovieRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync(int page, int pageSize)
        {
            _logger.LogInformation("Fetching movies: page {Page}, pageSize {PageSize}", page, pageSize);

            // AsNoTracking() — read-only query; EF Core does not track these entities.
            // Skip/Take — server-side pagination implemented in SQL (OFFSET/FETCH).
            return await _context.Movies
                .AsNoTracking()
                .OrderBy(m => m.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Movies.CountAsync();
        }

        public async Task<Movie?> GetByIdAsync(int movieId)
        {
            return await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MovieId == movieId);
        }

        public async Task<IEnumerable<Movie>> SearchByTitleAsync(string title)
        {
            // EF Core translates Contains() to SQL LIKE '%title%'
            return await _context.Movies
                .AsNoTracking()
                .Where(m => m.Title.Contains(title))
                .OrderBy(m => m.Title)
                .ToListAsync();
        }

        public async Task<Movie> CreateAsync(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Created movie with ID {MovieId}", movie.MovieId);
            return movie;
        }

        public async Task UpdateAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Updated movie with ID {MovieId}", movie.MovieId);
        }

        public async Task DeleteAsync(Movie movie)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Deleted movie with ID {MovieId}", movie.MovieId);
        }

        public async Task<bool> ExistsAsync(int movieId)
        {
            return await _context.Movies.AnyAsync(m => m.MovieId == movieId);
        }
    }
}
