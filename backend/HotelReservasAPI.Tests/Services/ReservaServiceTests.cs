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
			var options = new DbContextOptionsBuilder<HotelDbContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			_context = new HotelDbContext(options);
			_service = new ReservaService(_context);
		}

		[Fact]
		public async Task GetAllReservas_ShouldReturnEmptyList_WhenNoReservasExist()
		{
			// Act
			var result = await _service.GetAllReservas();

			// Assert
			Assert.Empty(result);
		}

		[Fact]
		public async Task GetAllReservas_ShouldReturnAllReservas()
		{
			// Arrange
			await _context.Reservas.AddRangeAsync(new List<Reserva>
			{
				new Reserva { NombreCliente = "Cliente 1", FechaEntrada = DateTime.Now, FechaSalida = DateTime.Now.AddDays(3) },
				new Reserva { NombreCliente = "Cliente 2", FechaEntrada = DateTime.Now.AddDays(1), FechaSalida = DateTime.Now.AddDays(4) }
			});
			await _context.SaveChangesAsync();

			// Act
			var result = await _service.GetAllReservas();

			// Assert
			Assert.Equal(2, result.Count);
		}

		[Fact]
		public async Task GetReservaById_ShouldReturnReserva_WhenExists()
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
			var result = await _service.GetReservaById(reserva.Id);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(reserva.NombreCliente, result.NombreCliente);
		}

		[Fact]
		public async Task GetReservaById_ShouldReturnNull_WhenNotExists()
		{
			// Act
			var result = await _service.GetReservaById(999);

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public async Task CreateReserva_ShouldAddReservaToDatabase()
		{
			// Arrange
			var reserva = new Reserva
			{
				NombreCliente = "Nuevo Cliente",
				FechaEntrada = DateTime.Now,
				FechaSalida = DateTime.Now.AddDays(3),
				Email = "cliente@test.com",
				Telefono = "123456789",
				NumeroHabitacion = 101
			};

			// Act
			var result = await _service.CreateReserva(reserva);

			// Assert
			Assert.NotNull(result);
			Assert.Equal(reserva.NombreCliente, result.NombreCliente);
			Assert.True(result.Id > 0);

			var savedReserva = await _context.Reservas.FindAsync(result.Id);
			Assert.NotNull(savedReserva); // Agregado para evitar warning
		}

		[Fact]
		public async Task UpdateReserva_ShouldUpdateExistingReserva()
		{
			// Arrange
			var reserva = new Reserva
			{
				NombreCliente = "Original Cliente",
				FechaEntrada = DateTime.Now,
				FechaSalida = DateTime.Now.AddDays(2)
			};

			_context.Reservas.Add(reserva);
			await _context.SaveChangesAsync();

			reserva.NombreCliente = "Cliente Actualizado";
			reserva.NumeroHabitacion = 202;

			// Act
			var result = await _service.UpdateReserva(reserva.Id, reserva);

			// Assert
			Assert.True(result);

			var updatedReserva = await _context.Reservas.FindAsync(reserva.Id);
			Assert.NotNull(updatedReserva); // Agregado para evitar warning
			Assert.Equal("Cliente Actualizado", updatedReserva.NombreCliente);
			Assert.Equal(202, updatedReserva.NumeroHabitacion);
		}

		[Fact]
		public async Task UpdateReserva_ShouldReturnFalse_WhenReservaDoesNotExist()
		{
			// Arrange
			var reserva = new Reserva { Id = 999, NombreCliente = "No existe" };

			// Act
			var result = await _service.UpdateReserva(999, reserva);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public async Task DeleteReserva_ShouldRemoveReserva()
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
			var result = await _service.DeleteReserva(reserva.Id);

			// Assert
			Assert.True(result);

			var deletedReserva = await _context.Reservas.FindAsync(reserva.Id);
			Assert.Null(deletedReserva);
		}

		[Fact]
		public async Task DeleteReserva_ShouldReturnFalse_WhenReservaDoesNotExist()
		{
			// Act
			var result = await _service.DeleteReserva(999);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public async Task ValidarDisponibilidad_ShouldReturnTrue_WhenRoomsAvailable()
		{
			// Arrange
			var fechaEntrada = DateTime.Now.AddDays(5);
			var fechaSalida = DateTime.Now.AddDays(7);

			// Act
			var result = await _service.ValidarDisponibilidad(fechaEntrada, fechaSalida);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public async Task ValidarDisponibilidad_ShouldReturnFalse_WhenNoRoomsAvailable()
		{
			// Arrange
			var fechaEntrada = DateTime.Now.AddDays(5);
			var fechaSalida = DateTime.Now.AddDays(7);

			// Crear 10 reservas para llenar disponibilidad
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

			// Act
			var result = await _service.ValidarDisponibilidad(fechaEntrada, fechaSalida);

			// Assert
			Assert.False(result);
		}
	}
}