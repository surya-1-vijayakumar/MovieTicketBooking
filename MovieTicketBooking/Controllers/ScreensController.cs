using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;
        private readonly ILogger<ScreensController> _logger;

        public ScreensController(IScreenService screenService, ILogger<ScreensController> logger)
        {
            _screenService = screenService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScreenResponseDto>>> GetAll()
        {
            var screens = await _screenService.GetAllScreensAsync();
            return Ok(screens);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ScreenResponseDto>> GetById(int id)
        {
            var screen = await _screenService.GetScreenByIdAsync(id);
            if (screen is null)
                return NotFound(new { message = $"Screen with ID {id} not found." });

            return Ok(screen);
        }

        [HttpGet("theatre/{theatreId:int}")]
        public async Task<ActionResult<IEnumerable<ScreenResponseDto>>> GetByTheatre(int theatreId)
        {
            var screens = await _screenService.GetScreensByTheatreAsync(theatreId);
            return Ok(screens);
        }

        [HttpPost]
        public async Task<ActionResult<ScreenResponseDto>> Create([FromBody] CreateScreenDto dto)
        {
            var created = await _screenService.CreateScreenAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.ScreenId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ScreenResponseDto>> Update(int id, [FromBody] UpdateScreenDto dto)
        {
            var updated = await _screenService.UpdateScreenAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _screenService.DeleteScreenAsync(id);
            return NoContent();
        }
    }
}

