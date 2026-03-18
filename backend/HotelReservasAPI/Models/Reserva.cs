namespace HotelReservasAPI.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }
        // Agrega más propiedades según necesites
        public int? NumeroHabitacion { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}