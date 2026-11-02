using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly ISeatRepository _seatRepository;
        private readonly ILogger<ShowService> _logger;

        public ShowService(
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository,
            ISeatRepository seatRepository,
            ILogger<ShowService> logger)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
            _seatRepository = seatRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<ShowResponseDto>> GetAllShowsAsync()
        {
            var shows = await _showRepository.GetAllAsync();
            return shows.Select(MapToDto);
        }

        public async Task<ShowResponseDto?> GetShowByIdAsync(int showId)
        {
            var show = await _showRepository.GetByIdAsync(showId);
            return show is null ? null : MapToDto(show);
        }

        public async Task<IEnumerable<ShowResponseDto>> GetShowsByMovieAsync(int movieId)
        {
            var movieExists = await _movieRepository.ExistsAsync(movieId);
            if (!movieExists)
                throw new KeyNotFoundException($"Movie with ID {movieId} was not found.");

            var shows = await _showRepository.GetByMovieIdAsync(movieId);
            return shows.Select(MapToDto);
        }

        public async Task<IEnumerable<ShowResponseDto>> GetShowsByTheatreAsync(int theatreId)
        {
            var shows = await _showRepository.GetByTheatreIdAsync(theatreId);
            return shows.Select(MapToDto);
        }

        public async Task<IEnumerable<ShowResponseDto>> GetShowsByDateAsync(DateTime date)
        {
            var shows = await _showRepository.GetByDateAsync(date);
            return shows.Select(MapToDto);
        }

        public async Task<ShowResponseDto> CreateShowAsync(CreateShowDto dto)
        {
            var movie = await _movieRepository.GetByIdAsync(dto.MovieId);
            if (movie is null)
                throw new KeyNotFoundException($"Movie with ID {dto.MovieId} was not found.");

            var screen = await _screenRepository.GetByIdAsync(dto.ScreenId);
            if (screen is null)
                throw new KeyNotFoundException($"Screen with ID {dto.ScreenId} was not found.");

            var endTime = dto.StartTime.Add(TimeSpan.FromMinutes(movie.DurationMinutes));

            var hasOverlap = await _showRepository.HasOverlappingShowAsync(
                dto.ScreenId, dto.ShowDate, dto.StartTime, endTime);

            if (hasOverlap)
                throw new InvalidOperationException(
                    $"A show already exists on screen {dto.ScreenId} that overlaps with {dto.StartTime:hh\\:mm} - {endTime:hh\\:mm} on {dto.ShowDate:yyyy-MM-dd}.");

            var show = new Show
            {
                MovieId = dto.MovieId,
                ScreenId = dto.ScreenId,
                ShowDate = dto.ShowDate.Date,
                StartTime = dto.StartTime,
                EndTime = endTime,
                BasePrice = dto.BasePrice,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var seats = (await _seatRepository.GetByScreenIdAsync(dto.ScreenId)).ToList();
            if (!seats.Any())
                throw new InvalidOperationException($"Screen {dto.ScreenId} has no seats. Please add seats before creating a show.");

            var showSeats = seats.Select(seat => new ShowSeat
            {
                SeatId = seat.SeatId,
                Status = ShowSeatStatus.Available,
                Price = seat.Price > 0 ? seat.Price : dto.BasePrice
            }).ToList();

            var created = await _showRepository.CreateAsync(show, showSeats);
            _logger.LogInformation("Show {ShowId} created for movie '{Title}' with {SeatCount} ShowSeats",
                created.ShowId, movie.Title, showSeats.Count);

            var withDetails = await _showRepository.GetByIdAsync(created.ShowId);
            return MapToDto(withDetails!);
        }

        public async Task<ShowResponseDto> UpdateShowAsync(int showId, UpdateShowDto dto)
        {
            var show = await _showRepository.GetByIdAsync(showId);
            if (show is null)
                throw new KeyNotFoundException($"Show with ID {showId} was not found.");

            var movie = await _movieRepository.GetByIdAsync(show.MovieId);
            var endTime = dto.StartTime.Add(TimeSpan.FromMinutes(movie!.DurationMinutes));

            var hasOverlap = await _showRepository.HasOverlappingShowAsync(
                show.ScreenId, dto.ShowDate, dto.StartTime, endTime, excludeShowId: showId);

            if (hasOverlap)
                throw new InvalidOperationException("The updated schedule overlaps with an existing show on this screen.");

            show.ShowDate = dto.ShowDate.Date;
            show.StartTime = dto.StartTime;
            show.EndTime = endTime;
            show.BasePrice = dto.BasePrice;
            show.IsActive = dto.IsActive;

            await _showRepository.UpdateAsync(show);
            var withDetails = await _showRepository.GetByIdAsync(showId);
            return MapToDto(withDetails!);
        }

        public async Task DeleteShowAsync(int showId)
        {
            var show = await _showRepository.GetByIdAsync(showId);
            if (show is null)
                throw new KeyNotFoundException($"Show with ID {showId} was not found.");

            await _showRepository.DeleteAsync(show);
        }

        private static ShowResponseDto MapToDto(Show s) => new()
        {
            ShowId = s.ShowId,
            MovieId = s.MovieId,
            MovieTitle = s.Movie?.Title ?? string.Empty,
            DurationMinutes = s.Movie?.DurationMinutes ?? 0,
            ScreenId = s.ScreenId,
            ScreenName = s.Screen?.ScreenName ?? string.Empty,
            TheatreId = s.Screen?.TheatreId ?? 0,
            TheatreName = s.Screen?.Theatre?.Name ?? string.Empty,
            ShowDate = s.ShowDate,
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            BasePrice = s.BasePrice,
            IsActive = s.IsActive,
            CreatedAt = s.CreatedAt,
            TotalSeats = s.ShowSeats?.Count ?? 0,
            AvailableSeats = s.ShowSeats?.Count(ss => ss.Status == ShowSeatStatus.Available) ?? 0
        };
    }
}

