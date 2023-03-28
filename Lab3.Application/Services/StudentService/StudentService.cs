using System.Text;
using Lab3.Application.Exceptions;
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

public class StudentService : IStudentService,IMessageProducer
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
        
        var desiredClass = _dbContext.Courses.Include(course => course.TeacherPerCourses)
            .FirstOrDefault(course => course.BranchTenantId == tenantId 
                                      && course.TeacherPerCourses.Any(perCourse => perCourse.Id == classId));
        
        /*var classes = from course in _dbContext.Courses
            join teacherPerCourses in _dbContext.TeacherPerCourses on course.Id equals teacherPerCourses.CourseId
            select new { Course = course, classId = teacherPerCourses.Id };*/

        if (desiredClass == null)
            throw new ClassNotFoundException("Class not found");

        
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
        newClassEnrollment.StudentId = _userIdentifierService.GetUserId();
        
        _dbContext.ClassEnrollments.Add(newClassEnrollment);
        await _dbContext.SaveChangesAsync();
        
        //SendMessage("sending test");

        return "Student x enrolled successfully in course y";
    }

    public void SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory { HostName = "localhost" };
        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare("notifications");

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: "notifications", body: body);
    }
}