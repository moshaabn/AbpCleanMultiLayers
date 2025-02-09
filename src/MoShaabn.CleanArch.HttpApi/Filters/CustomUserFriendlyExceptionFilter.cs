using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Volo.Abp;
using Volo.Abp.Validation;

namespace MoShaabn.CleanArch.Filters
{
    public class CustomUserFriendlyExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UserFriendlyException ex)
            {
                context.Result = new BadRequestObjectResult(new { ex.Message, ex.Code, ex.Details });
                context.ExceptionHandled = true; // Mark exception as handled
            }

            if (context.Exception is AbpValidationException validationException)
            {
                var errors = validationException.ValidationErrors
                    .GroupBy(
                        error => error.ErrorMessage == null ? "Error" : error.ErrorMessage.Split(' ')[0],
                        error => error.ErrorMessage)
                    .ToDictionary(
                        g => g.Key,
                        g => g.ToList()
                    );
                // Modify the default response format
                var errorDetails = new
                {
                    code = 400,
                    type = validationException.HelpLink,
                    message = "Validation failed.",
                    status = 400,
                    title = "One or more validation errors occurred.",
                    details = validationException.Message,
                    errors,
                    traceId = validationException.HelpLink,
                };

                // Return the modified response
                context.Result = new JsonResult(errorDetails)
                {
                    StatusCode = 400
                };

                context.ExceptionHandled = true; // Mark the exception as handled
            }
        }
    }
}