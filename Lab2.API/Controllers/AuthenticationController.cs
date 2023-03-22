using Lab2.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IFirebaseAuthService _firebaseAuthService;

    public AuthenticationController(IFirebaseAuthService firebaseAuthService)
    {
        _firebaseAuthService = firebaseAuthService;
    }
    
    [HttpPost("signin")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [AllowAnonymous]
    public async Task<ActionResult> SignIn(string email,string password)
    {
        return Ok(await _firebaseAuthService.SignIn(email, password));
    }
    
    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<ActionResult> SignUp(string email,string password)
    {
        return Ok(await _firebaseAuthService.SignUp(email, password));
    }
}