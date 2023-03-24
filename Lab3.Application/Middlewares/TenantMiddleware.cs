using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;

namespace Lab3.Application.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
 
    public TenantMiddleware(RequestDelegate next)
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
            var tenantId = jwtToken.Claims.FirstOrDefault(c => c.Type == "branchTenantId")?.Value;
            if (!string.IsNullOrEmpty(tenantId))
            {
                context.Items["TenantId"] = tenantId;
            }
        }
 
        await _next(context);
    }
}



