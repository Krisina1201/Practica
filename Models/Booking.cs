using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Booking
{
    public int BookingId { get; set; }

    public int PassengerId { get; set; }

    public string FlightId { get; set; } = null!;

    public string? TicketNumber { get; set; }

    public string SeatClass { get; set; } = null!;

    public int? LuggageCount { get; set; }

    public decimal TicketPrice { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? PaymentDeadline { get; set; }

    public DateTime? BookingTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Flight Flight { get; set; } = null!;

    public virtual Passenger Passenger { get; set; } = null!;

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
