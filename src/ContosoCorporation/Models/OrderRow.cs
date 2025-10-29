using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class OrderRow
{
    public long OrderKey { get; set; }

    public int LineNumber { get; set; }

    public int ProductKey { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal NetPrice { get; set; }

    public decimal UnitCost { get; set; }

    public virtual Order OrderKeyNavigation { get; set; } = null!;

    public virtual Product ProductKeyNavigation { get; set; } = null!;
}
