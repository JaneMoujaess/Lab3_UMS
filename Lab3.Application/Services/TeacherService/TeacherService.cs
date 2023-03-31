using Lab3.Application.Exceptions;
using Lab3.Application.Services.PublisherService;
using Lab3.Application.Services.StudentService;
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
    private readonly IPublisherService _publisherService;

    public TeacherService(UmsDbContext dbContext,IUserIdentifierService userIdentifierService,IPublisherService publisherService)
    {
        _dbContext = dbContext;
        _userIdentifierService = userIdentifierService;
        _publisherService = publisherService;
    }
    public async Task<long> TeachCourseHelper(long courseId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();

        var desiredCourse = await _dbContext.Courses.SingleOrDefaultAsync(course => course.BranchTenantId == tenantId 
            && course.Id==courseId);

        if (desiredCourse==null)
            throw new NotFoundException("Course not found");


        TeacherPerCourse newClass = new TeacherPerCourse();
        newClass.TeacherId = _userIdentifierService.GetUserId();
        newClass.CourseId = desiredCourse.Id;

        _dbContext.TeacherPerCourses.Add(newClass);
        await _dbContext.SaveChangesAsync();

        return newClass.Id;
    }
    public async Task<string> TeachCourse(long courseId,long sessionTimeId)
    {
        var classId = await TeachCourseHelper(courseId);

        if (_dbContext.SessionTimes.SingleOrDefault(sessionTime => sessionTime.Id == sessionTimeId) == null)
            throw new NotFoundException("Session time not found");
        
        TeacherPerCoursePerSessionTime newClassSession = new TeacherPerCoursePerSessionTime();
        newClassSession.TeacherPerCourseId = classId;
        newClassSession.SessionTimeId = sessionTimeId;
        
        _dbContext.TeacherPerCoursePerSessionTimes.Add(newClassSession);
        await _dbContext.SaveChangesAsync();

        return "Course x successfully registered by teacher y at specified time";
    }
    public async Task<string> CreateTimeSlot(DateTime startSession,DateTime endSession)
    {
        var userId = _userIdentifierService.GetUserId();
        SessionTime newSessionTime = new SessionTime();
        newSessionTime.StartTime = startSession;
        newSessionTime.EndTime = endSession;
        newSessionTime.TeacherId = userId;
        
        _dbContext.SessionTimes.Add(newSessionTime);
        await _dbContext.SaveChangesAsync();
        
        _publisherService.Publish("test","test");

        return "Session time successfully added";
    }

}