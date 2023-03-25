using Lab3.Application.Services.TenantProviderService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Lab3.Persistence.Exceptions;
using Microsoft.Extensions.Logging;

namespace Lab3.Application.Services.StudentService;

public class StudentService : IStudentService
{
    private readonly UmsDbContext _dbContext;
    private readonly ITenantProviderService _tenantProviderService;
    private readonly ILogger<StudentService> _logger;

    public StudentService(UmsDbContext dbContext,ITenantProviderService tenantProviderService,ILogger<StudentService> logger)
    {
        _dbContext = dbContext;
        _tenantProviderService = tenantProviderService;
        _logger = logger;
    }
    /*It takes a classId and checks in the database for a class given
    with the same Id. Note a class is the equivalent of a TeacherPerCourse.
    So I am looking for a course given by a specific teacher.
    This unique combo is relative to one and only one class with a particular classId.
    Also note that courses are being prefiltered with the branch/tenant in mind.*/
    public async Task<string> EnrollInClass(int classId)
    {
        var tenantId = _tenantProviderService.GetTenantId();
        
        var classes = from course in _dbContext.Courses
            join teacherPerCourses in _dbContext.TeacherPerCourses on course.Id equals teacherPerCourses.CourseId
            select new { Course = course, classId = teacherPerCourses.Id };

        var classIsAvailable =
            classes.Where(desiredClass => desiredClass.Course.BranchTenantId == tenantId)
                .Select(desiredClass => desiredClass.classId).Contains(classId);

        if (!classIsAvailable)
            throw new ClassNotFoundException("Class not found");

        ClassEnrollment newClassEnrollment = new ClassEnrollment(classId, 2);//Gotta get the studentId from the jwt somehow
        
        _dbContext.ClassEnrollments.Add(newClassEnrollment);
        await _dbContext.SaveChangesAsync();

        return "Student x enrolled successfully in course y";
    }
}