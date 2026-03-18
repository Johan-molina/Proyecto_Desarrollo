using HotelReservasAPI.Models;
using HotelReservasAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelReservasAPI.Services
{
    public class ReservaService
    {
        private readonly HotelDbContext _context;

        public ReservaService(HotelDbContext context)
        {
            _context = context;
        }

        // Obtener todas las reservas
        public async Task<List<Reserva>> GetAllReservas()
        {
            return await _context.Reservas.ToListAsync();
        }

        // Obtener una reserva por ID
        public async Task<Reserva?> GetReservaById(int id)
        {
            return await _context.Reservas.FindAsync(id);
        }

        // Crear una nueva reserva
        public async Task<Reserva> CreateReserva(Reserva reserva)
        {
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();
            return reserva;
        }

        // Actualizar una reserva
        public async Task<bool> UpdateReserva(int id, Reserva reserva)
        {
            if (id != reserva.Id)
                return false;

            _context.Entry(reserva).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ReservaExists(id))
                    return false;
                throw;
            }
        }

        // Eliminar una reserva
        public async Task<bool> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return false;

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            return true;
        }

        // Método adicional: Validar disponibilidad
        public async Task<bool> ValidarDisponibilidad(DateTime fechaEntrada, DateTime fechaSalida)
        {
            var reservasEnFecha = await _context.Reservas
                .Where(r => r.FechaEntrada >= fechaEntrada && r.FechaSalida <= fechaSalida)
                .CountAsync();

            return reservasEnFecha < 10;
        }

        private async Task<bool> ReservaExists(int id)
        {
            return await _context.Reservas.AnyAsync(e => e.Id == id);
        }
    }
}