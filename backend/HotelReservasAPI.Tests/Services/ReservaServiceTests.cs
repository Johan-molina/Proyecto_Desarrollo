using HotelReservasAPI.Models;
using HotelReservasAPI.Services;
using HotelReservasAPI.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelReservasAPI.Tests.Services
{
    public class ReservaServiceTests
    {
        private readonly HotelDbContext _context;
        private readonly ReservaService _service;

        public ReservaServiceTests()
        {
            // 🔹 Se configura una base de datos en memoria (simulación de BD real)
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // 🔹 Inicialización del contexto y servicio a probar
            _context = new HotelDbContext(options);
            _service = new ReservaService(_context);
        }

        [Fact]
        public async Task GetAllReservas_ShouldReturnEmptyList_WhenNoReservasExist()
        {
            // 🔹 Act: Se consulta la lista sin haber agregado datos
            var result = await _service.GetAllReservas();

            // 🔹 Assert: Se espera una lista vacía
            Assert.Empty(result);

            // ✔ Valida que el sistema no falle cuando no hay datos
        }

        [Fact]
        public async Task GetAllReservas_ShouldReturnAllReservas()
        {
            // 🔹 Arrange: Se insertan 2 reservas en la BD
            await _context.Reservas.AddRangeAsync(new List<Reserva>
            {
                new Reserva { NombreCliente = "Cliente 1", FechaEntrada = DateTime.Now, FechaSalida = DateTime.Now.AddDays(3) },
                new Reserva { NombreCliente = "Cliente 2", FechaEntrada = DateTime.Now.AddDays(1), FechaSalida = DateTime.Now.AddDays(4) }
            });
            await _context.SaveChangesAsync();

            // 🔹 Act: Se obtienen todas las reservas
            var result = await _service.GetAllReservas();

            // 🔹 Assert: Se valida que se devuelvan las 2 reservas
            Assert.Equal(2, result.Count);

            // ✔ Verifica lectura correcta desde la BD
        }

        [Fact]
        public async Task GetReservaById_ShouldReturnReserva_WhenExists()
        {
            // 🔹 Arrange: Se crea una reserva
            var reserva = new Reserva
            {
                NombreCliente = "Test Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2)
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // 🔹 Act: Se busca por ID
            var result = await _service.GetReservaById(reserva.Id);

            // 🔹 Assert: Debe retornar la reserva
            Assert.NotNull(result);
            Assert.Equal(reserva.NombreCliente, result.NombreCliente);

            // ✔ Verifica búsqueda correcta
        }

        [Fact]
        public async Task GetReservaById_ShouldReturnNull_WhenNotExists()
        {
            // 🔹 Act: Buscar un ID inexistente
            var result = await _service.GetReservaById(999);

            // 🔹 Assert: Debe retornar null
            Assert.Null(result);

            // ✔ Manejo correcto de datos inexistentes
        }

        [Fact]
        public async Task CreateReserva_ShouldAddReservaToDatabase()
        {
            // 🔹 Arrange: Se crea una nueva reserva
            var reserva = new Reserva
            {
                NombreCliente = "Nuevo Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(3),
                Email = "cliente@test.com",
                Telefono = "123456789",
                NumeroHabitacion = 101
            };

            // 🔹 Act: Se guarda la reserva
            var result = await _service.CreateReserva(reserva);

            // 🔹 Assert:
            Assert.NotNull(result); // ✔ No debe ser null
            Assert.Equal(reserva.NombreCliente, result.NombreCliente); // ✔ Datos correctos
            Assert.True(result.Id > 0); // ✔ ID generado

            // 🔹 Verificación directa en BD
            var savedReserva = await _context.Reservas.FindAsync(result.Id);
            Assert.NotNull(savedReserva);

            // ✔ Confirma persistencia real en BD
        }

        [Fact]
        public async Task UpdateReserva_ShouldUpdateExistingReserva()
        {
            // 🔹 Arrange: Se crea una reserva inicial
            var reserva = new Reserva
            {
                NombreCliente = "Original Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2)
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // 🔹 Se modifican los datos
            reserva.NombreCliente = "Cliente Actualizado";
            reserva.NumeroHabitacion = 202;

            // 🔹 Act: Se actualiza la reserva
            var result = await _service.UpdateReserva(reserva.Id, reserva);

            // 🔹 Assert:
            Assert.True(result); // ✔ Actualización exitosa

            var updatedReserva = await _context.Reservas.FindAsync(reserva.Id);
            Assert.NotNull(updatedReserva);
            Assert.Equal("Cliente Actualizado", updatedReserva.NombreCliente);
            Assert.Equal(202, updatedReserva.NumeroHabitacion);

            // ✔ Verifica que realmente se modificaron los datos
        }

        [Fact]
        public async Task UpdateReserva_ShouldReturnFalse_WhenReservaDoesNotExist()
        {
            // 🔹 Arrange: Reserva inexistente
            var reserva = new Reserva { Id = 999, NombreCliente = "No existe" };

            // 🔹 Act
            var result = await _service.UpdateReserva(999, reserva);

            // 🔹 Assert
            Assert.False(result);

            // ✔ Manejo correcto cuando no existe el registro
        }

        [Fact]
        public async Task DeleteReserva_ShouldRemoveReserva()
        {
            // 🔹 Arrange: Se crea una reserva
            var reserva = new Reserva
            {
                NombreCliente = "Cliente a eliminar",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(1)
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // 🔹 Act: Se elimina la reserva
            var result = await _service.DeleteReserva(reserva.Id);

            // 🔹 Assert:
            Assert.True(result); // ✔ Eliminación exitosa

            var deletedReserva = await _context.Reservas.FindAsync(reserva.Id);
            Assert.Null(deletedReserva); // ✔ Ya no existe

            // ✔ Verifica eliminación en BD
        }

        [Fact]
        public async Task DeleteReserva_ShouldReturnFalse_WhenReservaDoesNotExist()
        {
            // 🔹 Act
            var result = await _service.DeleteReserva(999);

            // 🔹 Assert
            Assert.False(result);

            // ✔ Manejo correcto de eliminación inválida
        }

        [Fact]
        public async Task ValidarDisponibilidad_ShouldReturnTrue_WhenRoomsAvailable()
        {
            // 🔹 Arrange: Fechas futuras sin reservas
            var fechaEntrada = DateTime.Now.AddDays(5);
            var fechaSalida = DateTime.Now.AddDays(7);

            // 🔹 Act
            var result = await _service.ValidarDisponibilidad(fechaEntrada, fechaSalida);

            // 🔹 Assert
            Assert.True(result);

            // ✔ Indica que hay habitaciones disponibles
        }

        [Fact]
        public async Task ValidarDisponibilidad_ShouldReturnFalse_WhenNoRoomsAvailable()
        {
            // 🔹 Arrange: Se llenan todas las habitaciones (ej: 10)
            var fechaEntrada = DateTime.Now.AddDays(5);
            var fechaSalida = DateTime.Now.AddDays(7);

            for (int i = 0; i < 10; i++)
            {
                _context.Reservas.Add(new Reserva
                {
                    NombreCliente = $"Cliente {i}",
                    FechaEntrada = fechaEntrada,
                    FechaSalida = fechaSalida
                });
            }
            await _context.SaveChangesAsync();

            // 🔹 Act
            var result = await _service.ValidarDisponibilidad(fechaEntrada, fechaSalida);

            // 🔹 Assert
            Assert.False(result);

            // ✔ Verifica lógica de límite de habitaciones
        }
    }
}