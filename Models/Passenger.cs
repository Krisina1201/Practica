using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Passenger
{
    public int PassengerId { get; set; }

    public string PassportNumber { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? LuggageInfo { get; set; }

    public string? SalesInfo { get; set; }

    public string? BuyerUser { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<PassengerFlight> PassengerFlights { get; set; } = new List<PassengerFlight>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
