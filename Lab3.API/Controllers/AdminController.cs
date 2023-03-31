using Lab3.Application.DTOs;
using Lab3.Application.Mediators.AdminMediator;
using Lab3.Application.Mediators.AdminMediator.AdminCommands;
using Lab3.Application.Mediators.CourseMediator;
using Lab3.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace Lab3.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IMediator mediator,ILogger<AdminController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("CreateCourse")]
    [Authorize(Policy = "AdminPermission")]
    public async Task<ActionResult<List<Course>>> CreateCourse(CourseDtoRequest newCourse)
    {
        _logger.LogInformation("authorized");
        return Ok(await _mediator.Send(new CreateCourseCommand { NewCourse = newCourse }));
    }

    [HttpGet("commonStudents")]
    [Authorize(Policy = "AdminPermission")]
    public async Task<ActionResult<List<string>>> GetCommonStudents(long firstTeacherId, long secondTeacherId)
    {
        return Ok(await _mediator.Send(new GetCommonStudentsQuery { FirstTeacherId = firstTeacherId,SecondTeacherId = secondTeacherId}));
    }
    
    [HttpGet("genderStatistics")]
    [Authorize(Policy = "AdminPermission")]
    public async Task<ActionResult<List<string>>> GetGenderStatistics()
    {
        return Ok(await _mediator.Send(new GetGenderStatisticsQuery()));
    }
    
    
}