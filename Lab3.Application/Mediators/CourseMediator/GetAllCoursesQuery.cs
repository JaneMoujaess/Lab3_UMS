using Lab3.Application.DTOs;
using Lab3.Application.Services.CourseService;
using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.CourseMediator;

public class GetAllCoursesQuery:IRequest<List<CourseDTOResponse>>
{
    
}

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<CourseDTOResponse>>
{
    private readonly ICourseService _courseService;

    public GetAllCoursesQueryHandler(ICourseService courseService)
    {
        _courseService = courseService;
    }
    public async Task<List<CourseDTOResponse>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _courseService.GetAllCourses();
    }
}