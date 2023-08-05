using EStore.Common.API.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EStore.Common.API.Utils
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ControllerExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ILogger<ControllerExceptionFilterAttribute>? logger = GetLogger(context.HttpContext);

            logger?.LogError("Exception: {exception}", context.Exception.ToString());

            string errorMessage;

            if (IsDevelopmentEnvironment())
                errorMessage = context.Exception.ToString();
            else
                errorMessage = "Internal error occurred.";

            ErrorResponse errorResponse = new(
                context.HttpContext,
                code: "INTERNAL_SERVER_ERROR",
                message: errorMessage);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        private ILogger<ControllerExceptionFilterAttribute>? GetLogger(HttpContext httpContext)
        {
            object? logger = httpContext.RequestServices.GetService(typeof(ILogger<ControllerExceptionFilterAttribute>));
            return (ILogger<ControllerExceptionFilterAttribute>?)logger;
        }

        private bool IsDevelopmentEnvironment()
        {
            return string.Equals(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"), "Development");
        }
    }
}
