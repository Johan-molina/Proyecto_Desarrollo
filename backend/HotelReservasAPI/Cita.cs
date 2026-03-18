using System;
using System.Collections.Generic;

namespace HotelReservasAPI;

public partial class Cita
{
    public int Id { get; set; }

    public string NombreCliente { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public string? Motivo { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }
}
