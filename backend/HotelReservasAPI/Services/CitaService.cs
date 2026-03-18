using HotelReservasAPI.Models;
using HotelReservasAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservasAPI.Services
{
    public class CitaService
    {
        private readonly HotelDbContext _context;

        public CitaService(HotelDbContext context)
        {
            _context = context;
        }

        // Obtener todas las citas
        public async Task<List<Cita>> GetAllCitas()
        {
            return await _context.Citas.ToListAsync();
        }

        // Obtener una cita por ID
        public async Task<Cita?> GetCitaById(int id)
        {
            return await _context.Citas.FindAsync(id);
        }

        // Crear una nueva cita
        public async Task<Cita> CreateCita(Cita cita)
        {
            _context.Citas.Add(cita);
            await _context.SaveChangesAsync();
            return cita;
        }

        // Actualizar una cita
        public async Task<bool> UpdateCita(int id, Cita cita)
        {
            if (id != cita.Id)
                return false;

            _context.Entry(cita).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CitaExists(id))
                    return false;
                throw;
            }
        }

        // Eliminar una cita
        public async Task<bool> DeleteCita(int id)
        {
            var cita = await _context.Citas.FindAsync(id);
            if (cita == null)
                return false;

            _context.Citas.Remove(cita);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<bool> CitaExists(int id)
        {
            return await _context.Citas.AnyAsync(e => e.Id == id);
        }
    }
}