using System;
using System.Collections.Generic;

namespace Finally.Models;

public partial class Grade
{
    public int Id { get; set; }

    public int? StudentId { get; set; }

    public int? ClassId { get; set; }

    public int? TeacherId { get; set; }

    public double? Grade1 { get; set; }

    public DateOnly? DayOfGrade { get; set; }

    public virtual Class? Class { get; set; }

    public virtual Student? Student { get; set; }

    public virtual Teacher? Teacher { get; set; }
}
