using Lab3.Application.DTOs;
using Lab3.Application.Mediators.AdminMediator.AdminCommands;
using Lab3.Application.Mediators.StudentMediator;
using Lab3.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController:ControllerBase
{
    private readonly IMediator _mediator;

    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("Enroll")]
    [Authorize(Policy = "StudentPermission")]
    public async Task<ActionResult<List<Course>>> EnrollInCourse(long classId)
    {
        return Ok(await _mediator.Send(new EnrollInCourseCommand() { ClassId = classId }));
    }
}