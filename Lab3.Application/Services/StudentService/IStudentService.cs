using Lab3.Domain.Models;

namespace Lab3.Application.Services.StudentService;

public interface IStudentService
{
    public Task<string> EnrollInClass(long classId);
}