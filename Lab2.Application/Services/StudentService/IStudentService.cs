using Lab2.Domain.Models;

namespace Lab2.Application.Services.StudentService;

public interface IStudentService
{
    public Task<string> EnrollInCourse(Course course);
}