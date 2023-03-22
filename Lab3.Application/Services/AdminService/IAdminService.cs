using Lab3.Domain.Models;

namespace Lab3.Application.Services.AdminService;

public interface IAdminService
{
    public Task<List<Course>> GetAllCourses();
    public Task<List<Course>> CreateCourse(Course newCourse);
}