using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelReservasAPI.Data;
using HotelReservasAPI.Models;
using HotelReservasAPI.Services;

namespace HotelReservasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservasController : ControllerBase
    {
        private readonly ReservaService _reservaService;

        public ReservasController(ReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            try
            {
                var reservas = await _reservaService.GetAllReservas();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _reservaService.GetReservaById(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return reserva;
        }

        // POST: api/Reservas
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            try
            {
                var nuevaReserva = await _reservaService.CreateReserva(reserva);
                return CreatedAtAction(nameof(GetReserva), new { id = nuevaReserva.Id }, nuevaReserva);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        // PUT: api/Reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return BadRequest();
            }

            var actualizado = await _reservaService.UpdateReserva(id, reserva);
            if (!actualizado)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var eliminado = await _reservaService.DeleteReserva(id);
            if (!eliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}