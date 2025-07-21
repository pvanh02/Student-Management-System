using System;
using System.Collections.Generic;

namespace Finally.Models;

public partial class Teacher
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public int? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? Subject { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Role? Role { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
