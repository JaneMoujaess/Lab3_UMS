using Lab3.Application.DTOs;
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

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateCourse")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<List<Course>>> CreateCourse(CourseDTO newCourse)
    {
        return Ok(await _mediator.Send(new CreateCourseCommand { newCourse = newCourse }));
    }
}