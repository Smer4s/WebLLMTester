using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Uply.Web.Filters;

public class ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(context.Exception, "Unhandled exception occurred");

        var statusCode = HttpStatusCode.InternalServerError;
        var message = "An unexpected error occurred";

        if (context.Exception is not null)
        {
            statusCode = HttpStatusCode.BadRequest;
            message = context.Exception.Message;
        }

        context.Result = new ObjectResult(new
        {
            error = message,
            status = (int)statusCode
        })
        {
            StatusCode = (int)statusCode
        };

        context.ExceptionHandled = true;
    }
}

