using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class PassengerFlight
{
    public int PassengerFlightId { get; set; }

    public int PassengerId { get; set; }

    public string FlightId { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public int? UserByueId { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;

    public virtual User? UserByue { get; set; }
}
