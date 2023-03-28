using Lab3.Domain.Models;

namespace Lab3.Application.Services.TeacherService;

public interface ITeacherService
{
    public Task<string> TeachCourse(long classId,long sessionTimeId);
    public Task<string> CreateTimeSlot(DateTime startSession,DateTime endSession);
}