using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Lab3.Application.Middlewares;

public class UserIdentificationMiddleware
{
    private readonly RequestDelegate _next;
 
    public UserIdentificationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var authHeader = context.Request.Headers["Authorization"].ToString();
            var token = authHeader.Substring("Bearer ".Length).Trim();
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                context.Items["userId"] = userId;
            }
        }
        await _next(context);
    }
}