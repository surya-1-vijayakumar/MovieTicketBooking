using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;
        private readonly ILogger<TheatresController> _logger;

        public TheatresController(ITheatreService theatreService, ILogger<TheatresController> logger)
        {
            _theatreService = theatreService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TheatreResponseDto>>> GetAll()
        {
            var theatres = await _theatreService.GetAllTheatresAsync();
            return Ok(theatres);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TheatreResponseDto>> GetById(int id)
        {
            var theatre = await _theatreService.GetTheatreByIdAsync(id);
            if (theatre is null)
                return NotFound(new { message = $"Theatre with ID {id} not found." });

            return Ok(theatre);
        }

        [HttpPost]
        public async Task<ActionResult<TheatreResponseDto>> Create([FromBody] CreateTheatreDto dto)
        {
            var created = await _theatreService.CreateTheatreAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TheatreId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TheatreResponseDto>> Update(int id, [FromBody] UpdateTheatreDto dto)
        {
            var updated = await _theatreService.UpdateTheatreAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _theatreService.DeleteTheatreAsync(id);
            return NoContent();
        }
    }
}

