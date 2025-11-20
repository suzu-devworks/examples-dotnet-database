using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class CurrencyExchange1
{
    public DateOnly Date { get; set; }

    public string FromCurrency { get; set; } = null!;

    public string ToCurrency { get; set; } = null!;

    public double Exchange { get; set; }
}
