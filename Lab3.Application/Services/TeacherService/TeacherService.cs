using Lab3.Domain.Models;

namespace Lab3.Application.Services.TeacherService;

public class TeacherService:ITeacherService
{
    public Task<string> TeachCourse(Course course)
    {
        throw new NotImplementedException();
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