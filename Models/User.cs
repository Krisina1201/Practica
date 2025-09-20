using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class User
{
    public int UserId { get; set; }

    public string? UserPassword { get; set; }

    public string? Email { get; set; }

    public int? TicketId { get; set; }

    public string? Role { get; set; }

    public string? Fio { get; set; }

    public string? PassportData { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<PassengerFlight> PassengerFlights { get; set; } = new List<PassengerFlight>();
}
