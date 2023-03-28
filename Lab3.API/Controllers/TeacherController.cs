using Lab3.Application.Mediators.TeacherMediator;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController:ControllerBase
{
    private readonly IMediator _mediator;

    public TeacherController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost("teachCourse")]
    [Authorize(Policy = "TeacherPermission")]
    public async Task<ActionResult<string>> TeachCourse(long courseId,long sessionTimeId)
    {
        return Ok(await _mediator.Send(new TeachCourseCommand() { CourseId = courseId,SessionTimeId = sessionTimeId}));
    }

    [HttpPost("createTimeSlot")]
    [Authorize(Policy = "TeacherPermission")]
    public async Task<ActionResult<string>> CreateTimeSlot(DateTime startSession, DateTime endSession)
    {
        return Ok(await _mediator.Send(new CreateTimeSlotCommand()
            { startSession = startSession, endSession = endSession }));
    }
}