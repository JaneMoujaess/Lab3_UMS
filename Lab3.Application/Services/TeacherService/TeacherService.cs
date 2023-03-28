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
    
    /*public async Task<string> TeachCourse(long courseId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();

        var teachableCourses = await _dbContext.Courses.Where(course => course.BranchTenantId == tenantId).ToListAsync();

        var desiredCourseId = teachableCourses.SingleOrDefault(desiredCourse => desiredCourse.Id==courseId).Id;

        var courseIsAvailable = desiredCourseId != null && (desiredCourseId == 0 ? false : true);
           
        if (!courseIsAvailable)
            throw new CourseNotFoundException("Course not found");


        TeacherPerCourse newClass = new TeacherPerCourse();
        newClass.TeacherId = _userIdentifierService.GetUserId();
        newClass.CourseId = desiredCourseId;

        _dbContext.TeacherPerCourses.Add(newClass);
        await _dbContext.SaveChangesAsync();

        return "Course x successfully registered by teacher y";
    }*/
    public async Task<long> TeachCourseHelper(long courseId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();

        var teachableCourses = await _dbContext.Courses.Where(course => course.BranchTenantId == tenantId).ToListAsync();

        var desiredCourseId = teachableCourses.SingleOrDefault(desiredCourse => desiredCourse.Id==courseId).Id;

        var courseIsAvailable = desiredCourseId != null && (desiredCourseId == 0 ? false : true);
           
        if (!courseIsAvailable)
            throw new CourseNotFoundException("Course not found");


        TeacherPerCourse newClass = new TeacherPerCourse();
        newClass.TeacherId = _userIdentifierService.GetUserId();
        newClass.CourseId = desiredCourseId;

        _dbContext.TeacherPerCourses.Add(newClass);
        await _dbContext.SaveChangesAsync();

        return newClass.Id;
    }
    public async Task<string> TeachCourse(long courseId,long sessionTimeId)
    {

        var classId = await TeachCourseHelper(courseId);
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

        return "Session time successfully added";
    }

}