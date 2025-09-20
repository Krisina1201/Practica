using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Reservation
{
    public int ReservationId { get; set; }

    public int PassengerId { get; set; }

    public string FlightId { get; set; } = null!;

    public decimal Price { get; set; }

    public int PaymentTerm { get; set; }

    public DateTime ReservationTime { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;
}
