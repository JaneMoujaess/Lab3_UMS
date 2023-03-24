using System;
using System.Collections.Generic;

namespace Lab3.Domain.Models;

public partial class BranchTenant
{
    public long Id { get; set; }

    public string Location { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; } = new List<Course>();
}
