using HotelReservasAPI.Models;
using HotelReservasAPI.Services;
using HotelReservasAPI.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelReservasAPI.Tests.Services
{
    public class CitaServiceTests
    {
        private readonly HotelDbContext _context;
        private readonly CitaService _service;

        public CitaServiceTests()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HotelDbContext(options);
            _service = new CitaService(_context);
        }

        [Fact]
        public async Task GetAllCitas_ShouldReturnAllCitas()
        {
            // Arrange
            await _context.Citas.AddRangeAsync(new List<Cita>
            {
                new Cita { NombreCliente = "Cliente 1", Fecha = DateTime.Now, Motivo = "Consulta" },
                new Cita { NombreCliente = "Cliente 2", Fecha = DateTime.Now.AddDays(1), Motivo = "Reunión" }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllCitas();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CreateCita_ShouldAddCitaToDatabase()
        {
            // Arrange
            var cita = new Cita
            {
                NombreCliente = "Nuevo Cliente",
                Fecha = DateTime.Now.AddDays(2),
                Motivo = "Consulta general",
                Email = "cliente@test.com",
                Telefono = "123456789"
            };

            // Act
            var result = await _service.CreateCita(cita);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(cita.NombreCliente, result.NombreCliente);

            var savedCita = await _context.Citas.FindAsync(result.Id);
            Assert.NotNull(savedCita);
            Assert.Equal(cita.Motivo, savedCita.Motivo);
        }
    }
}