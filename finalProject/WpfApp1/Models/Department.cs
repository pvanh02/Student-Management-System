using System;
using System.Collections.Generic;

namespace Finally.Models;

public partial class Department
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
}
