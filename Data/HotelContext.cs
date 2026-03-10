using HotelReservasAPI.Models;

namespace HotelReservasAPI.Data
{
    public static class HotelContext
    {
        public static List<Reserva> Reservas = new List<Reserva>();
        public static List<Cita> Citas = new List<Cita>();
    }
}