using Lab3.Domain.Models;
using Lab3.Persistence;
using Microsoft.EntityFrameworkCore;

public class AdminService : IAdminService
{
    private readonly UmsDbContext _dbContext;

    public AdminService(UmsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Course>> CreateCourse(Course newCourse)
    {
        _dbContext.Courses.Add(newCourse);
        await _dbContext.SaveChangesAsync();
        return await _dbContext.Courses.ToListAsync();
    }

    public async Task<List<Course>> GetAllCourses()
    {
        return await _dbContext.Courses.ToListAsync();
    }
}