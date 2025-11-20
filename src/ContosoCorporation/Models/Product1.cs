using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Product1
{
    public int ProductKey { get; set; }

    public string ProductCode { get; set; } = null!;

    public string? ProductName { get; set; }

    public string? Manufacturer { get; set; }

    public string? Brand { get; set; }

    public string? Color { get; set; }

    public string? WeightUnitMeasure { get; set; }

    public double? Weight { get; set; }

    public decimal? UnitCost { get; set; }

    public decimal? UnitPrice { get; set; }

    public int? SubcategoryCode { get; set; }

    public string? Subcategory { get; set; }

    public int? CategoryCode { get; set; }

    public string? Category { get; set; }
}
