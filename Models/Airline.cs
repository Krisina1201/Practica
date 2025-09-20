using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Airline
{
    public string AirlineCode { get; set; } = null!;

    public string AirlineName { get; set; } = null!;

    public DateOnly? FoundationDate { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateOnly? EstablishmentDate { get; set; }

    public virtual ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();
}
