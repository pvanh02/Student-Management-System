using System;
using System.Collections.Generic;

namespace Finally.Models;

public partial class Schedule
{
    public int Id { get; set; }

    public string? DayOfWeeks { get; set; }

    public int? Slot { get; set; }

    public int? ClassId { get; set; }

    public int? TeacherId { get; set; }

    public DateOnly? TimeOfWeek { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
