using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Customer
{
    public int CustomerKey { get; set; }

    public int GeoAreaKey { get; set; }

    public DateOnly? StartDt { get; set; }

    public DateOnly? EndDt { get; set; }

    public string? Continent { get; set; }

    public string? Gender { get; set; }

    public string? Title { get; set; }

    public string? GivenName { get; set; }

    public string? MiddleInitial { get; set; }

    public string? Surname { get; set; }

    public string? StreetAddress { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? StateFull { get; set; }

    public string? ZipCode { get; set; }

    public string? Country { get; set; }

    public string? CountryFull { get; set; }

    public DateOnly Birthday { get; set; }

    public int Age { get; set; }

    public string? Occupation { get; set; }

    public string? Company { get; set; }

    public string? Vehicle { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
