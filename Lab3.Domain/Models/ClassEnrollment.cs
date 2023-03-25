using System;
using System.Collections.Generic;

namespace Lab3.Domain.Models;

public partial class ClassEnrollment
{
    public long Id { get; set; }

    public long ClassId { get; set; }

    public long StudentId { get; set; }

    public virtual TeacherPerCourse Class { get; set; } = null!;

    public virtual User Student { get; set; } = null!;

    public ClassEnrollment(long classId, long studentId)
    {
        ClassId = classId;
        StudentId = studentId;
    }
}
