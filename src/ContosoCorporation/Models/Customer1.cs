using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Customer1
{
    public int CustomerKey { get; set; }

    public string? Gender { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? StateCode { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public string? CountryCode { get; set; }

    public string? Country { get; set; }

    public string? Continent { get; set; }

    public DateOnly Birthday { get; set; }

    public int Age { get; set; }
}
