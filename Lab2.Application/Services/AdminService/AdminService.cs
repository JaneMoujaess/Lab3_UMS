using Lab2.Domain.Models;
using Lab2.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Application.Services.AdminService;

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