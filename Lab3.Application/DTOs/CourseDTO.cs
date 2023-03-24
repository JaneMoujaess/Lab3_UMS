using NpgsqlTypes;

namespace Lab3.Application.DTOs;

public class CourseDTO
{
    public string? Name { get; set; }

    public int? MaxStudentsNumber { get; set; }

    public NpgsqlRange<DateOnly>? EnrolmentDateRange { get; set; }

    //public long BranchTenantId { get; set; }
}