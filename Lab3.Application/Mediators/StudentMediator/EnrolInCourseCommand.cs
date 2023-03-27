using Lab3.Application.Mediators.AdminMediator.AdminCommands;
using Lab3.Application.Services.StudentService;
using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.StudentMediator;

public class EnrollInCourseCommand:IRequest<string>
{
    public long classId { set; get; }
}

public class EnrollInCourseCommandHandler : IRequestHandler<EnrollInCourseCommand, string>
{
    private readonly IStudentService _studentService;

    public EnrollInCourseCommandHandler(IStudentService studentService)
    {
        _studentService = studentService;
        
    }public async Task<string> Handle(EnrollInCourseCommand request, CancellationToken cancellationToken)
    {
        return await _studentService.EnrollInClass(request.classId);
    }
}