using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Store1
{
    public int StoreKey { get; set; }

    public int StoreCode { get; set; }

    public string? Country { get; set; }

    public string? State { get; set; }

    public string? Name { get; set; }

    public int? SquareMeters { get; set; }

    public DateOnly? OpenDate { get; set; }

    public DateOnly? CloseDate { get; set; }

    public string? Status { get; set; }
}
