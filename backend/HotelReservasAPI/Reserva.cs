using System;
using System.Collections.Generic;

namespace HotelReservasAPI;

public partial class Reserva
{
    public int Id { get; set; }

    public string NombreCliente { get; set; } = null!;

    public DateTime FechaEntrada { get; set; }

    public DateTime FechaSalida { get; set; }

    public int? NumeroHabitacion { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }
}
