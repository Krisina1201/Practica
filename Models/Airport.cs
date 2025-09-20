using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Airport
{
    public string AirportCode { get; set; } = null!;

    public string AirportName { get; set; } = null!;

    public string AirportNameRu { get; set; } = null!;

    public int CityId { get; set; }

    public string? IataCode { get; set; }

    public string? IcaoCode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<Flight> FlightArrivalAirportNavigations { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightDepartureAirportNavigations { get; set; } = new List<Flight>();
}
