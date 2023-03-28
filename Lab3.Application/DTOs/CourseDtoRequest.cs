using Lab3.Common;
using NpgsqlTypes;

namespace Lab3.Application.DTOs;

public class CourseDtoRequest
{
    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }
    public Date StartEnrollment { get; set; }
    public Date EndEnrollment { get; set; }
    
}