using Lab3.Application.Services.TeacherService;
using MediatR;

namespace Lab3.Application.Mediators.TeacherMediator;

public class TeachCourseCommand:IRequest<string>
{
    public long courseId { set; get; }
}

public class TeachCourseCommandHandler : IRequestHandler<TeachCourseCommand, string>
{
    private readonly ITeacherService _teacherService;

    public TeachCourseCommandHandler(ITeacherService teacherService)
    {
        _teacherService = teacherService;
        
    }public async Task<string> Handle(TeachCourseCommand request, CancellationToken cancellationToken)
    {
        return await _teacherService.TeachCourse(request.courseId);
    }
}