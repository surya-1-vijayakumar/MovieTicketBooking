using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Movies;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
        {
            _movieService = movieService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<PagedMovieResponseDto>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _movieService.GetAllMoviesAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieResponseDto>> GetById(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie is null)
                return NotFound(new { message = $"Movie with ID {id} not found." });

            return Ok(movie);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<MovieSummaryDto>>> Search([FromQuery] string title)
        {
            var movies = await _movieService.SearchMoviesByTitleAsync(title);
            return Ok(movies);
        }

        [HttpPost]
        public async Task<ActionResult<MovieResponseDto>> Create([FromBody] CreateMovieDto dto)
        {
            var created = await _movieService.CreateMovieAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.MovieId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MovieResponseDto>> Update(int id, [FromBody] UpdateMovieDto dto)
        {
            var updated = await _movieService.UpdateMovieAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _movieService.DeleteMovieAsync(id);
            return NoContent();
        }
    }
}

