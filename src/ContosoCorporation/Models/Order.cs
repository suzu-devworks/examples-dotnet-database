using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Order
{
    public long OrderKey { get; set; }

    public int CustomerKey { get; set; }

    public int StoreKey { get; set; }

    public DateOnly OrderDate { get; set; }

    public DateOnly DeliveryDate { get; set; }

    public string CurrencyCode { get; set; } = null!;

    public virtual Customer CustomerKeyNavigation { get; set; } = null!;

    public virtual ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();

    public virtual Store StoreKeyNavigation { get; set; } = null!;
}
