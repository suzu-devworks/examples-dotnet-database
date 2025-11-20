using System;
using System.Collections.Generic;

namespace ContosoCorporation.Models;

public partial class Date
{
    public DateOnly Date1 { get; set; }

    public string DateKey { get; set; } = null!;

    public int Year { get; set; }

    public string YearQuarter { get; set; } = null!;

    public int YearQuarterNumber { get; set; }

    public string Quarter { get; set; } = null!;

    public string YearMonth { get; set; } = null!;

    public string YearMonthShort { get; set; } = null!;

    public int YearMonthNumber { get; set; }

    public string Month { get; set; } = null!;

    public string MonthShort { get; set; } = null!;

    public int MonthNumber { get; set; }

    public string DayofWeek { get; set; } = null!;

    public string DayofWeekShort { get; set; } = null!;

    public int DayofWeekNumber { get; set; }

    public bool WorkingDay { get; set; }

    public int WorkingDayNumber { get; set; }
}
