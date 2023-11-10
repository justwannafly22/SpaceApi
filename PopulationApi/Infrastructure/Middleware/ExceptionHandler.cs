using MinimalApi.Boundary;
using Serilog;
using System.Net;

namespace MinimalApi.Infrastructure.Middleware;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ArgumentNullException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (ArgumentException ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex.Message, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, string message, HttpStatusCode statusCode)
    {
        Log.Error(message, "Error occured - ");

        var response = httpContext.Response;

        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;

        await response.WriteAsync(new BaseResponseModel
        { 
            Message = message,
            StatusCode = statusCode 
        }.ToString());
    }
}
