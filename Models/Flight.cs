using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Flight
{
    public string FlightId { get; set; } = null!;

    public string AirplaneId { get; set; } = null!;

    public string AirlineCode { get; set; } = null!;

    public string DepartureAirport { get; set; } = null!;

    public string ArrivalAirport { get; set; } = null!;

    public DateTime ScheduledDeparture { get; set; }

    public DateTime ScheduledArrival { get; set; }

    public DateTime? ActualDeparture { get; set; }

    public DateTime? ActualArrival { get; set; }

    public int? PassengerCount { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? Economprise { get; set; }

    public int? Bisnesprice { get; set; }

    public virtual Airline AirlineCodeNavigation { get; set; } = null!;

    public virtual Airplane Airplane { get; set; } = null!;

    public virtual Airport ArrivalAirportNavigation { get; set; } = null!;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Airport DepartureAirportNavigation { get; set; } = null!;

    public virtual ICollection<PassengerFlight> PassengerFlights { get; set; } = new List<PassengerFlight>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
