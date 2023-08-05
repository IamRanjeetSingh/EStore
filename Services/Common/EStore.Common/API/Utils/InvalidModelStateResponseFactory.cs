using EStore.Common.API.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace EStore.Common.API.Utils
{
    public static class InvalidModelStateResponseFactory
    {
        public static IActionResult CreateResponse(ActionContext actionContext)
        {   
            ErrorResponse response = new(
                actionContext.HttpContext, 
                code: "BAD_REQUEST", 
                message: "One or more errors were found in the submitted request.",
                errors: GetErrors(actionContext));
            
            return new BadRequestObjectResult(response);
        }

        private static string[] GetErrors(ActionContext actionContext)
        {
            return actionContext.ModelState.Values
                .SelectMany(value => 
                    value.Errors
                        .Select(error => 
                            error.ErrorMessage))
                .ToArray();
        }
    }
}
