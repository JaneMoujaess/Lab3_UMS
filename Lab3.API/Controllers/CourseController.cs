using Lab3.Application.Mediators.CourseMediator;
using Lab3.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace Lab3.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CourseController:ControllerBase
{
    private readonly IMediator _mediator;

    public CourseController(IMediator mediator)
    {
        _mediator = mediator;
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