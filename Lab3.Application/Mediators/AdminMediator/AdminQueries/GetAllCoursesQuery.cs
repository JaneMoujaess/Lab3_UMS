﻿using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.AdminMediator.AdminQueries;

public class GetAllCoursesQuery:IRequest<List<Course>>
{
    
}

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, List<Course>>
{
    private readonly IAdminService _adminService;

    public GetAllCoursesQueryHandler(IAdminService adminService)
    {
        _adminService = adminService;
    }
    public async Task<List<Course>> Handle(GetAllCoursesQuery request, CancellationToken cancellationToken)
    {
        return await _adminService.GetAllCourses();
    }
}