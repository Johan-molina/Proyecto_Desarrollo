using Microsoft.EntityFrameworkCore;
using HotelReservasAPI.Models;

namespace HotelReservasAPI.Data
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions<HotelDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}