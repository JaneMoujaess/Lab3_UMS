using System.Text;
using Lab3.Application.DTOs;
using Lab3.Application.Exceptions;
using Lab3.Application.Services.PublisherService;
using Lab3.Application.Services.UserIdentifierService;
using Lab3.Domain.Models;
using Lab3.Persistence;
using Lab3.Persistence.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Lab3.Application.Services.StudentService;

public class StudentService : IStudentService
{
    private readonly UmsDbContext _dbContext;
    private readonly IUserIdentifierService _userIdentifierService;
    private readonly IPublisherService _publisherService;

    public StudentService(UmsDbContext dbContext, IUserIdentifierService userIdentifierService,IPublisherService publisherService)
    {
        _dbContext = dbContext;
        _userIdentifierService = userIdentifierService;
        _publisherService = publisherService;
    }
    public async Task<string> EnrollInClass(long classId)
    {
        var tenantId = await _userIdentifierService.GetTenantId();
        
        var desiredClass = _dbContext.Courses.Include(course => course.TeacherPerCourses)
            .FirstOrDefault(course => course.BranchTenantId == tenantId 
                                      && course.TeacherPerCourses.Any(perCourse => perCourse.Id == classId));
        
        //Alternative inner join approach
        /*var classes = from course in _dbContext.Courses
            join teacherPerCourses in _dbContext.TeacherPerCourses on course.Id equals teacherPerCourses.CourseId
            select new { Course = course, classId = teacherPerCourses.Id };*/

        if (desiredClass == null)
            throw new NotFoundException("Class not found");

        
        //Check for max number of students
        var numOfStudentsEnrolled =
            _dbContext.ClassEnrollments.Where(desiredClass => desiredClass.ClassId == classId).Count();
        
        if (numOfStudentsEnrolled >= desiredClass.MaxStudentsNumber)
            throw new FullClassException("Maximum number of students reached");

        //check for date of enrollment validity
        DateOnly dateOfEnrollment = DateOnly.FromDateTime(DateTime.Now);
        if (!(desiredClass.EnrolmentDateRange.Value.LowerBound <= dateOfEnrollment
              && desiredClass.EnrolmentDateRange.Value.UpperBound >= dateOfEnrollment))
            throw new DateNotInEnrollmentRange("Can't enroll at the current date");
            
        
        ClassEnrollment newClassEnrollment = new ClassEnrollment();
        newClassEnrollment.ClassId = classId;
        var studentId=_userIdentifierService.GetUserId();
        newClassEnrollment.StudentId = studentId;
        
        _dbContext.ClassEnrollments.Add(newClassEnrollment);
        await _dbContext.SaveChangesAsync();

        var classes =
            await _dbContext.TeacherPerCourses.SingleOrDefaultAsync(desiredClass => desiredClass.Id == classId);
        var courseId = classes.CourseId;
        var teacherId = classes.TeacherId;
        
        _publisherService.Publish("notifications",new NotificationDto(){TeacherId = teacherId,StudentId = studentId,CourseId = courseId});

        return "Student x enrolled successfully in course y";
    }
}