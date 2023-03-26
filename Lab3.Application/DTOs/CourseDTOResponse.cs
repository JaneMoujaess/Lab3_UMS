using NpgsqlTypes;

namespace Lab3.Application.DTOs;

public class CourseDTOResponse
{
    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }

    public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }
}