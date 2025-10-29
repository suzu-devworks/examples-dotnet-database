using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Product
{
    public int ProductKey { get; set; }

    public string ProductCode { get; set; } = null!;

    public string? ProductName { get; set; }

    public string? Manufacturer { get; set; }

    public string? Brand { get; set; }

    public string? Color { get; set; }

    public string? WeightUnit { get; set; }

    public double? Weight { get; set; }

    public decimal? Cost { get; set; }

    public decimal? Price { get; set; }

    public int? CategoryKey { get; set; }

    public string? CategoryName { get; set; }

    public int? SubCategoryKey { get; set; }

    public string? SubCategoryName { get; set; }

    public virtual ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
