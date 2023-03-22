using Lab2.Domain.Models;

namespace Lab2.Application.Services.AdminService;

public interface IAdminService
{
    public Task<List<Course>> GetAllCourses();
    public Task<List<Course>> CreateCourse(Course newCourse);
}