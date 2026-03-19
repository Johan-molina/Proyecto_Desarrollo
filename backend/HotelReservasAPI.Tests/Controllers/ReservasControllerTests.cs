using HotelReservasAPI.Controllers;
using HotelReservasAPI.Models;
using HotelReservasAPI.Services;
using HotelReservasAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HotelReservasAPI.Tests.Controllers
{
    public class ReservasControllerTests
    {
        private readonly ReservasController _controller;
        private readonly ReservaService _service;
        private readonly HotelDbContext _context;

        public ReservasControllerTests()
        {
            // 🔹 Se configura una base de datos en memoria para pruebas (no afecta la real)
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HotelDbContext(options);

            // 🔹 Se instancia el servicio que contiene la lógica de negocio
            _service = new ReservaService(_context);

            // 🔹 Se crea el controlador con el servicio inyectado
            _controller = new ReservasController(_service);
        }

        [Fact]
        public async Task GetReservas_ShouldReturnOkResult_WithListOfReservas()
        {
            // 🔹 Arrange: Se agregan reservas de prueba a la BD en memoria
            await _context.Reservas.AddRangeAsync(new List<Reserva>
            {
                new Reserva { NombreCliente = "Cliente 1", FechaEntrada = DateTime.Now, FechaSalida = DateTime.Now.AddDays(2) },
                new Reserva { NombreCliente = "Cliente 2", FechaEntrada = DateTime.Now.AddDays(1), FechaSalida = DateTime.Now.AddDays(3) }
            });
            await _context.SaveChangesAsync();

            // 🔹 Act: Se llama al método del controlador
            var result = await _controller.GetReservas();

            // 🔹 Assert: Se valida que retorne 200 OK con la lista de reservas
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Reserva>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var reservas = Assert.IsAssignableFrom<IEnumerable<Reserva>>(okResult.Value);
            Assert.Equal(2, reservas.Count());
        }

        [Fact]
        public async Task GetReserva_ShouldReturnOkResult_WithReserva()
        {
            // 🔹 Arrange: Se crea y guarda una reserva
            var reserva = new Reserva
            {
                NombreCliente = "Test Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2)
            };
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // 🔹 Act: Se consulta la reserva por ID
            var result = await _controller.GetReserva(reserva.Id);

            // 🔹 Assert: Se valida que retorne la reserva correctamente
            var actionResult = Assert.IsType<ActionResult<Reserva>>(result);
            var returnValue = Assert.IsType<Reserva>(actionResult.Value);
            Assert.Equal(reserva.NombreCliente, returnValue.NombreCliente);
        }

        [Fact]
        public async Task GetReserva_ShouldReturnNotFound_WhenReservaDoesNotExist()
        {
            // 🔹 Act: Se busca una reserva inexistente
            var result = await _controller.GetReserva(999);

            // 🔹 Assert: Se valida que retorne 404 NotFound
            var actionResult = Assert.IsType<ActionResult<Reserva>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostReserva_ShouldReturnCreatedAtAction_WithNewReserva()
        {
            // 🔹 Arrange: Se crea una nueva reserva
            var reserva = new Reserva
            {
                NombreCliente = "Nuevo Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(3),
                Email = "nuevo@test.com"
            };

            // 🔹 Act: Se envía la reserva al controlador (POST)
            var result = await _controller.PostReserva(reserva);

            // 🔹 Assert:
            // ✔ Retorna 201 Created
            // ✔ Retorna el objeto creado
            // ✔ Se genera un ID automáticamente
            var actionResult = Assert.IsType<ActionResult<Reserva>>(result);
            var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Reserva>(createdResult.Value);

            Assert.Equal(reserva.NombreCliente, returnValue.NombreCliente);
            Assert.True(returnValue.Id > 0);
            Assert.Equal(nameof(_controller.GetReserva), createdResult.ActionName);
        }

        [Fact]
        public async Task PutReserva_ShouldReturnNoContent_WhenUpdateIsSuccessful()
        {
            // 🔹 Arrange: Se crea una reserva inicial
            var reserva = new Reserva
            {
                NombreCliente = "Cliente Original",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2)
            };
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // 🔹 Se modifica el nombre
            reserva.NombreCliente = "Cliente Actualizado";

            // 🔹 Act: Se envía la actualización
            var result = await _controller.PutReserva(reserva.Id, reserva);

            // 🔹 Assert:
            // ✔ Retorna 204 NoContent
            // ✔ El dato fue actualizado en BD
            Assert.IsType<NoContentResult>(result);

            var updatedReserva = await _context.Reservas.FindAsync(reserva.Id);
            Assert.NotNull(updatedReserva);
            Assert.Equal("Cliente Actualizado", updatedReserva.NombreCliente);
        }

        [Fact]
        public async Task PutReserva_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // 🔹 Arrange: IDs diferentes
            var reserva = new Reserva { Id = 1, NombreCliente = "Test" };

            // 🔹 Act: Se intenta actualizar con ID diferente
            var result = await _controller.PutReserva(2, reserva);

            // 🔹 Assert: Debe retornar 400 BadRequest
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteReserva_ShouldReturnNoContent_WhenDeleteIsSuccessful()
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
            var result = await _controller.DeleteReserva(reserva.Id);

            // 🔹 Assert:
            // ✔ Retorna 204 NoContent
            // ✔ La reserva ya no existe en BD
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Reservas.FindAsync(reserva.Id));
        }

        [Fact]
        public async Task DeleteReserva_ShouldReturnNotFound_WhenReservaDoesNotExist()
        {
            // 🔹 Act: Se intenta eliminar una reserva inexistente
            var result = await _controller.DeleteReserva(999);

            // 🔹 Assert: Debe retornar 404 NotFound
            Assert.IsType<NotFoundResult>(result);
        }
    }
}