using Lab3.Application.Services.CourseService;
using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.CourseMediator;

public class GetAllCoursesQuery:IRequest<List<Course>>
{
    
}

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<Course>>
{
    private readonly ICourseService _courseService;

    public GetAllCoursesQueryHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }
    public async Task<List<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _courseService.GetAllCourses();
    }
}