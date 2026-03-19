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
            // 🔹 Se crea una base de datos en memoria (simula la BD real sin afectarla)
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // 🔹 Se inicializa el contexto con esa base de datos temporal
            _context = new HotelDbContext(options);

            // 🔹 Se instancia el servicio que vamos a probar
            _service = new CitaService(_context);
        }

        [Fact]
        public async Task GetAllCitas_ShouldReturnAllCitas()
        {
            // 🔹 Arrange (Preparación):
            // Se agregan dos citas de prueba en la base de datos en memoria
            await _context.Citas.AddRangeAsync(new List<Cita>
            {
                new Cita { NombreCliente = "Cliente 1", Fecha = DateTime.Now, Motivo = "Consulta" },
                new Cita { NombreCliente = "Cliente 2", Fecha = DateTime.Now.AddDays(1), Motivo = "Reunión" }
            });

            // Se guardan los cambios en la BD simulada
            await _context.SaveChangesAsync();

            // 🔹 Act (Acción):
            // Se ejecuta el método del servicio que queremos probar
            var result = await _service.GetAllCitas();

            // 🔹 Assert (Validación):
            // Se verifica que el servicio retorne exactamente 2 citas
            Assert.Equal(2, result.Count);

            // ✔ Con esto comprobamos que:
            // - El servicio sí consulta la BD
            // - Retorna todos los registros correctamente
        }

        [Fact]
        public async Task CreateCita_ShouldAddCitaToDatabase()
        {
            // 🔹 Arrange:
            // Se crea un objeto cita con datos de prueba
            var cita = new Cita
            {
                NombreCliente = "Nuevo Cliente",
                Fecha = DateTime.Now.AddDays(2),
                Motivo = "Consulta general",
                Email = "cliente@test.com",
                Telefono = "123456789"
            };

            // 🔹 Act:
            // Se llama al método que crea una cita en la BD
            var result = await _service.CreateCita(cita);

            // 🔹 Assert:

            // ✔ Se valida que el resultado no sea nulo
            Assert.NotNull(result);

            // ✔ Se valida que la BD haya generado un ID (guardado exitoso)
            Assert.True(result.Id > 0);

            // ✔ Se valida que los datos retornados coincidan con los enviados
            Assert.Equal(cita.NombreCliente, result.NombreCliente);

            // 🔹 Validación extra importante:
            // Se busca la cita directamente en la base de datos
            var savedCita = await _context.Citas.FindAsync(result.Id);

            // ✔ Se verifica que realmente se guardó en la BD
            Assert.NotNull(savedCita);

            // ✔ Se valida que el motivo coincida
            Assert.Equal(cita.Motivo, savedCita.Motivo);

            // ✔ Con esto comprobamos que:
            // - El servicio inserta correctamente en la BD
            // - Los datos no se pierden ni se alteran
            // - La persistencia funciona correctamente
        }
    }
}