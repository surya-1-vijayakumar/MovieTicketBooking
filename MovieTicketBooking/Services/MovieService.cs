using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMovieRepository movieRepository, ILogger<MovieService> logger)
        {
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<PagedMovieResponseDto> GetAllMoviesAsync(int page, int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var movies = await _movieRepository.GetAllAsync(page, pageSize);
            var totalCount = await _movieRepository.GetTotalCountAsync();

            return new PagedMovieResponseDto
            {
                Movies = movies.Select(MapToSummaryDto),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<MovieResponseDto?> GetMovieByIdAsync(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);
            return movie is null ? null : MapToResponseDto(movie);
        }

        public async Task<IEnumerable<MovieSummaryDto>> SearchMoviesByTitleAsync(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Search title cannot be empty.");

            var movies = await _movieRepository.SearchByTitleAsync(title.Trim());
            return movies.Select(MapToSummaryDto);
        }

        public async Task<MovieResponseDto> CreateMovieAsync(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title.Trim(),
                Description = dto.Description?.Trim(),
                Genre = dto.Genre.Trim(),
                Language = dto.Language.Trim(),
                DurationMinutes = dto.DurationMinutes,
                ReleaseDate = dto.ReleaseDate,
                Rating = dto.Rating,
                PosterUrl = dto.PosterUrl?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _movieRepository.CreateAsync(movie);
            _logger.LogInformation("Movie '{Title}' created with ID {MovieId}", created.Title, created.MovieId);
            return MapToResponseDto(created);
        }

        public async Task<MovieResponseDto> UpdateMovieAsync(int movieId, UpdateMovieDto dto)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie is null)
                throw new KeyNotFoundException($"Movie with ID {movieId} was not found.");

            movie.Title = dto.Title.Trim();
            movie.Description = dto.Description?.Trim();
            movie.Genre = dto.Genre.Trim();
            movie.Language = dto.Language.Trim();
            movie.DurationMinutes = dto.DurationMinutes;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.Rating = dto.Rating;
            movie.PosterUrl = dto.PosterUrl?.Trim();
            movie.IsActive = dto.IsActive;

            await _movieRepository.UpdateAsync(movie);
            return MapToResponseDto(movie);
        }

        public async Task DeleteMovieAsync(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);
            if (movie is null)
                throw new KeyNotFoundException($"Movie with ID {movieId} was not found.");

            await _movieRepository.DeleteAsync(movie);
        }

        private static MovieResponseDto MapToResponseDto(Movie movie) => new()
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Description = movie.Description,
            Genre = movie.Genre,
            Language = movie.Language,
            DurationMinutes = movie.DurationMinutes,
            ReleaseDate = movie.ReleaseDate,
            Rating = movie.Rating,
            PosterUrl = movie.PosterUrl,
            IsActive = movie.IsActive,
            CreatedAt = movie.CreatedAt
        };

        private static MovieSummaryDto MapToSummaryDto(Movie movie) => new()
        {
            MovieId = movie.MovieId,
            Title = movie.Title,
            Genre = movie.Genre,
            Language = movie.Language,
            DurationMinutes = movie.DurationMinutes,
            Rating = movie.Rating,
            IsActive = movie.IsActive
        };
    }
}

