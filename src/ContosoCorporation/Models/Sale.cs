using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Sale
{
    public long OrderKey { get; set; }

    public int LineNumber { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public int CustomerKey { get; set; }

    public int StoreKey { get; set; }

    public int ProductKey { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal NetPrice { get; set; }

    public decimal UnitCost { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public double ExchangeRate { get; set; }

    public virtual Customer CustomerKeyNavigation { get; set; } = null!;

    public virtual Product ProductKeyNavigation { get; set; } = null!;

    public virtual Store StoreKeyNavigation { get; set; } = null!;
}
