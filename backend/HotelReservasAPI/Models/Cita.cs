namespace HotelReservasAPI.Models
{
    public class Cita
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        // Agrega más propiedades según necesites
        public string? Motivo { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
    }
}