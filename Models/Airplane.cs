using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Airplane
{
    public string AirplaneId { get; set; } = null!;

    public int ModelId { get; set; }

    public string AirlineCode { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateOnly RegistrationDate { get; set; }

    public virtual Airline AirlineCodeNavigation { get; set; } = null!;

    public virtual ICollection<Flight> Flights { get; set; } = new List<Flight>();

    public virtual AirplaneModel Model { get; set; } = null!;
}
