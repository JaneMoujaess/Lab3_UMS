using Microsoft.AspNetCore.Mvc;

namespace Lab3.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeacherController:ControllerBase
{
    [HttpPost("teachCourse")]
    public async Task<ActionResult<string>> teachCourse(long courseId)
    {
        return Ok("test");
    }
}