using Lab3.Application.Services.UserIdentifierService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Lab3.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab3.Application.Services.TeacherService;

public class TeacherService : ITeacherService
{
    private readonly UmsDbContext _dbContext;
    private readonly IUserIdentifierService _userIdentifierService;

    public TeacherService(UmsDbContext dbContext,IUserIdentifierService userIdentifierService)
    {
        _dbContext = dbContext;
        _userIdentifierService = userIdentifierService;
    }
    
    public async Task<string> TeachCourse(long courseId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();

        var teachableCourses = await _dbContext.Courses.Where(course => course.BranchTenantId == tenantId).ToListAsync();

        var desiredCourseId = teachableCourses.SingleOrDefault(desiredCourse => desiredCourse.Id);

        var courseIsAvailable = desiredCourseId == 0 ? false : true;
           
        if (!courseIsAvailable)
            throw new CourseNotFoundException("Course not found");
        

        //TeacherPerCourse newClass = new TeacherPerCourse(courseId,);
        
        /*dbContext.TeacherPerCourses.Add(newClassEnrollment);
        await _dbContext.SaveChangesAsync();*/

        return "Student x enrolled successfully in course y";
    }

    public Task<string> CreateTimeSlot(DateTime timeSlot)
    {
        throw new NotImplementedException();
    }

    public Task<string> AssignCourseToTimeSlot(Course course, DateTime timeSlot)
    {
        throw new NotImplementedException();
    }
}