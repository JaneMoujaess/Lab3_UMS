using Lab3.Domain.Models;

namespace Lab3.Application.Services.CourseService;

public interface ICourseService
{
    public Task<List<Course>> GetAllCourses();
}