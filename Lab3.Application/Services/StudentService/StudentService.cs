using Lab3.Application.Services.UserIdentifierService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Lab3.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Lab3.Application.Services.StudentService;

public class StudentService : IStudentService
{
    private readonly UmsDbContext _dbContext;
    private readonly ILogger<StudentService> _logger;
    private readonly IUserIdentifierService _userIdentifierService;

    public StudentService(UmsDbContext dbContext, IUserIdentifierService userIdentifierService,ILogger<StudentService> logger)
    {
        _dbContext = dbContext;
        _userIdentifierService = userIdentifierService;
        _logger = logger;
    }
    /*It takes a classId and checks in the database for a class given
    with the same Id. Note a class is the equivalent of a TeacherPerCourse.
    So I am looking for a course given by a specific teacher.
    This unique combo is relative to one and only one class with a particular classId.
    Also note that courses are being prefiltered with the branch/tenant in mind.
    
    I should later also check for number of students<max number of students
    and that date at which the student enrolled is contained in the enrollment date range of the course
    */
    public async Task<string> EnrollInClass(long classId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();
        
        var courses = _dbContext.Courses.Include(course => course.TeacherPerCourses)
            .FirstOrDefault(course => course.BranchTenantId == tenantId 
                                      && course.TeacherPerCourses.Any(perCourse => perCourse.Id == classId));
        
        /*var classes = from course in _dbContext.Courses
            join teacherPerCourses in _dbContext.TeacherPerCourses on course.Id equals teacherPerCourses.CourseId
            select new { Course = course, classId = teacherPerCourses.Id };*/

        if (courses == null)
        {
            throw new ClassNotFoundException("Class not found");
        }
        
        ClassEnrollment newClassEnrollment = new ClassEnrollment(classId, _userIdentifierService.GetUserId());
        
        _dbContext.ClassEnrollments.Add(newClassEnrollment);
        await _dbContext.SaveChangesAsync();
        
        channel.ExchangeDeclare("logs", ExchangeType.Fanout);

        return "Student x enrolled successfully in course y";
    }
}