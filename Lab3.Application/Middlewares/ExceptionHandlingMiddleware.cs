using System.Net;
using Firebase.Auth;
using Lab3.Persistence.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Lab3.Application.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ClassNotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(ex.Message);
        }
        catch (FirebaseAuthException ex)
        {
            // Handle the Firebase exception
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            // Handle all other exceptions
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);

            //await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}