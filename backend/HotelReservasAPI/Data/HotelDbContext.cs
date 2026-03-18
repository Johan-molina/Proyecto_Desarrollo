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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // SQLite no soporta ciertas cosas como HasDefaultValueSql de la misma forma
            // Pero las configuraciones básicas funcionan perfectamente

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreCliente)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreCliente)
                    .IsRequired()
                    .HasMaxLength(100);
            });
        }
    }
}