using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Country
{
    public string CountryCode { get; set; } = null!;

    public string CountryName { get; set; } = null!;

    public string CountryNameRu { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();
}
