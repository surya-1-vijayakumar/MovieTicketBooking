using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly ILogger<SeatsController> _logger;

        public SeatsController(ISeatService seatService, ILogger<SeatsController> logger)
        {
            _seatService = seatService;
            _logger = logger;
        }

        [HttpGet("screen/{screenId:int}")]
        public async Task<ActionResult<IEnumerable<SeatResponseDto>>> GetByScreen(int screenId)
        {
            var seats = await _seatService.GetSeatsByScreenAsync(screenId);
            return Ok(seats);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<SeatResponseDto>> GetById(int id)
        {
            var seat = await _seatService.GetSeatByIdAsync(id);
            if (seat is null)
                return NotFound(new { message = $"Seat with ID {id} not found." });

            return Ok(seat);
        }

        [HttpPost]
        public async Task<ActionResult<SeatResponseDto>> Create([FromBody] CreateSeatDto dto)
        {
            var created = await _seatService.CreateSeatAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.SeatId }, created);
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<SeatResponseDto>>> CreateBulk([FromBody] BulkCreateSeatDto dto)
        {
            var created = await _seatService.CreateBulkSeatsAsync(dto);
            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<SeatResponseDto>> Update(int id, [FromBody] UpdateSeatDto dto)
        {
            var updated = await _seatService.UpdateSeatAsync(id, dto);
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _seatService.DeleteSeatAsync(id);
            return NoContent();
        }
    }
}

