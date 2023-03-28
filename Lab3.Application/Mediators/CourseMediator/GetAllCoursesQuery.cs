using Lab3.Application.DTOs;
using Lab3.Application.Services.CourseService;
using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.CourseMediator;

public class GetAllCoursesQuery:IRequest<List<CourseDtoResponse>>
{
    
}

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDtoResponse>>
{
    private readonly ICourseService _courseService;

    public GetAllCoursesQueryHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }
    public async Task<List<CourseDtoResponse>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _courseService.GetAllCourses();
    }
}