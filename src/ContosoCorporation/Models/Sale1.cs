using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Sale1
{
    public long OrderNumber { get; set; }

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
}
