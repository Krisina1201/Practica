using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class AirplaneModel
{
    public int ModelId { get; set; }

    public string ModelName { get; set; } = null!;

    public int MaxPassengers { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Airplane> Airplanes { get; set; } = new List<Airplane>();
}
