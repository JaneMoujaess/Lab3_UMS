using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace Lab3.Domain.Models;

public partial class Course
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }

    public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }

    public long BranchTenantId { get; set; }

    public virtual BranchTenant BranchTenant { get; set; } = null!;

    public virtual ICollection<TeacherPerCourse> TeacherPerCourses { get; } = new List<TeacherPerCourse>();
}
