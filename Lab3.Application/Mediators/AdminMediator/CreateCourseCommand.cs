﻿using Lab3.Application.DTOs;
using Lab3.Domain.Models;
using MediatR;

namespace Lab3.Application.Mediators.AdminMediator.AdminCommands;

public class CreateCourseCommand : IRequest<List<Course>>
{
    public CourseDtoRequest NewCourse { set; get; }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand,List<Course>>
{
    private readonly IAdminService _adminService;

    public CreateCourseCommandHandler(IAdminService adminService)
    {
        _adminService = adminService;
    }
    public async Task<List<Course>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        return await _adminService.CreateCourse(request.NewCourse);
    }
}