using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Store
{
    public int StoreKey { get; set; }

    public int StoreCode { get; set; }

    public int GeoAreaKey { get; set; }

    public string? CountryCode { get; set; }

    public string? CountryName { get; set; }

    public string? State { get; set; }

    public DateOnly? OpenDate { get; set; }

    public DateOnly? CloseDate { get; set; }

    public string? Description { get; set; }

    public int? SquareMeters { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
