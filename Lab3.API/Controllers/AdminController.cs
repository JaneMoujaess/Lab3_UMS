using Lab3.Application.Mediators.AdminMediator.AdminCommands;
using Lab3.Application.Mediators.AdminMediator.AdminQueries;
using Lab3.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

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

    [HttpPost]
    [Authorize] //(Role.Admin) to restrict access to users who've been already authenticated and have a role of Admin => custom authorization
    //Though, I have to find a way to perhaps pass a jwt token to ensure authorization thus enabling the method.
    public async Task<ActionResult<List<Course>>> CreateCourse(Course newCourse)
    {
        return Ok(await _mediator.Send(new CreateCourseCommand { newCourse = newCourse }));
    }

    [HttpGet]
    [EnableQuery]
    [Authorize]
    public async Task<ActionResult<List<Course>>> GetAllCourses()
    {
        return Ok(await _mediator.Send(new GetAllCoursesQuery()));
    }

    /*[HttpGet]
    [EnableQuery]
    public async Task<ActionResult<List<Course>>> GetAllCourses()
    {
        return Ok(await _mediator.Send(new GetOdataQuery()
        {
            Type = typeof(Course)
        }));
    }*/
}