using System;
using System.Collections.Generic;

namespace Demochka.Modelss;

public partial class Sale
{
    public int SaleId { get; set; }

    public int BookingId { get; set; }

    public decimal PaymentAmount { get; set; }

    public string PaymentMethod { get; set; } = null!;

    public DateTime? PaymentDate { get; set; }

    public string? TransactionReference { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Booking Booking { get; set; } = null!;
}
