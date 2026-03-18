using Microsoft.AspNetCore.Mvc;
using HotelReservasAPI.Models;
using HotelReservasAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HotelReservasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitasController : ControllerBase
    {
        private readonly CitaService _citaService;

        public CitasController(CitaService citaService)
        {
            _citaService = citaService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Cita>>> GetCitas()
        {
            return await _citaService.GetAllCitas();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cita>> GetCita(int id)
        {
            var cita = await _citaService.GetCitaById(id);
            if (cita == null)
                return NotFound();
            return cita;
        }

        [HttpPost]
        public async Task<ActionResult<Cita>> PostCita(Cita cita)
        {
            var nuevaCita = await _citaService.CreateCita(cita);
            return CreatedAtAction(nameof(GetCita), new { id = nuevaCita.Id }, nuevaCita);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCita(int id, Cita cita)
        {
            if (id != cita.Id)
                return BadRequest();

            var actualizado = await _citaService.UpdateCita(id, cita);
            if (!actualizado)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCita(int id)
        {
            var eliminado = await _citaService.DeleteCita(id);
            if (!eliminado)
                return NotFound();

            return NoContent();
        }
    }
}