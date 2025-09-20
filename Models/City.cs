using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class City
{
    public int CityId { get; set; }

    public string CityName { get; set; } = null!;

    public string CityNameRu { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string Timezone { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Airport> Airports { get; set; } = new List<Airport>();

    public virtual Country CountryCodeNavigation { get; set; } = null!;
}
