using Lab3.Domain.Models;

namespace Lab3.Application.Services.TeacherService;

public interface ITeacherService
{
    public Task<string> TeachCourse(long courseId);
    public Task<string> CreateTimeSlot(DateTime timeSlot);
    public Task<string> AssignCourseToTimeSlot(Course course, DateTime timeSlot);
}