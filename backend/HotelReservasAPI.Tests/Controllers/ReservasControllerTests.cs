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
            // Configurar DbContext en memoria
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new HotelDbContext(options);
            _service = new ReservaService(_context);
            _controller = new ReservasController(_service);
        }

        [Fact]
        public async Task GetReservas_ShouldReturnOkResult_WithListOfReservas()
        {
            // Arrange
            await _context.Reservas.AddRangeAsync(new List<Reserva>
            {
                new Reserva { NombreCliente = "Cliente 1", FechaEntrada = DateTime.Now, FechaSalida = DateTime.Now.AddDays(2) },
                new Reserva { NombreCliente = "Cliente 2", FechaEntrada = DateTime.Now.AddDays(1), FechaSalida = DateTime.Now.AddDays(3) }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetReservas();

            // Assert - Corregido para manejar correctamente ActionResult
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Reserva>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var reservas = Assert.IsAssignableFrom<IEnumerable<Reserva>>(okResult.Value);
            Assert.Equal(2, reservas.Count());
        }

        [Fact]
        public async Task GetReserva_ShouldReturnOkResult_WithReserva()
        {
            // Arrange
            var reserva = new Reserva
            {
                NombreCliente = "Test Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2)
            };
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetReserva(reserva.Id);

            // Assert - Corregido
            var actionResult = Assert.IsType<ActionResult<Reserva>>(result);
            var returnValue = Assert.IsType<Reserva>(actionResult.Value);
            Assert.Equal(reserva.NombreCliente, returnValue.NombreCliente);
        }

        [Fact]
        public async Task GetReserva_ShouldReturnNotFound_WhenReservaDoesNotExist()
        {
            // Act
            var result = await _controller.GetReserva(999);

            // Assert - Corregido
            var actionResult = Assert.IsType<ActionResult<Reserva>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostReserva_ShouldReturnCreatedAtAction_WithNewReserva()
        {
            // Arrange
            var reserva = new Reserva
            {
                NombreCliente = "Nuevo Cliente",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(3),
                Email = "nuevo@test.com"
            };

            // Act
            var result = await _controller.PostReserva(reserva);

            // Assert - Corregido
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
            // Arrange
            var reserva = new Reserva
            {
                NombreCliente = "Cliente Original",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(2)
            };
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            reserva.NombreCliente = "Cliente Actualizado";

            // Act
            var result = await _controller.PutReserva(reserva.Id, reserva);

            // Assert - Corregido
            Assert.IsType<NoContentResult>(result);

            var updatedReserva = await _context.Reservas.FindAsync(reserva.Id);
            Assert.NotNull(updatedReserva); // Agregado para evitar warning CS8602
            Assert.Equal("Cliente Actualizado", updatedReserva.NombreCliente);
        }

        [Fact]
        public async Task PutReserva_ShouldReturnBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var reserva = new Reserva { Id = 1, NombreCliente = "Test" };

            // Act
            var result = await _controller.PutReserva(2, reserva);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteReserva_ShouldReturnNoContent_WhenDeleteIsSuccessful()
        {
            // Arrange
            var reserva = new Reserva
            {
                NombreCliente = "Cliente a eliminar",
                FechaEntrada = DateTime.Now,
                FechaSalida = DateTime.Now.AddDays(1)
            };
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteReserva(reserva.Id);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Reservas.FindAsync(reserva.Id));
        }

        [Fact]
        public async Task DeleteReserva_ShouldReturnNotFound_WhenReservaDoesNotExist()
        {
            // Act
            var result = await _controller.DeleteReserva(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}