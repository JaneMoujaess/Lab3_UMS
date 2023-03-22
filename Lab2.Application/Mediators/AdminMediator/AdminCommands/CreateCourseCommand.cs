using Lab2.Application.Services.AdminService;
using Lab2.Domain.Models;
using MediatR;

namespace Lab2.Application.Mediators.AdminMediator.AdminCommands;

public class CreateCourseCommand:IRequest<List<Course>>
{
    public Course newCourse { set; get; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, List<Course>>
{
    private readonly IAdminService _adminService;

    public CreateCourseCommandHandler(IAdminService adminService)
    {
        _adminService = adminService;
    }
    public async Task<List<Course>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        return await _adminService.CreateCourse(request.newCourse);
    }
}
