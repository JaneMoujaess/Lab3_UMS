using NpgsqlTypes;

namespace Lab3.Application.DTOs;

public class CourseDTO
{
    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }
    public Date StartEnrollment { get; set; }
    public Date EndEnrollment { get; set; }
    
}

public class Date
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Day { get; set; }
}