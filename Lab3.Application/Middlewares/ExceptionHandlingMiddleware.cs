using System.Net;
using Firebase.Auth;
using Lab3.Application.Exceptions;
using Lab3.Persistence.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Lab3.Application.Middlewares;

// todo: move to presentation
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
        catch (NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (FullClassException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (DateNotInEnrollmentRange ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (PostgresException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (FirebaseAuthException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(ex.Message);
        }
        catch (Exception ex)
        {
            // Handle all other exceptions
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Stack Trace:\n"+ex.StackTrace+"\n\n"+ex.Message);

            //await context.Response.WriteAsync("An unexpected error occurred.");
        }
    }
}