using BugStoreDAL.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;

namespace BugStoreAPI.Filters
{
    public class BugStoreExceptionFilter : IExceptionFilter
    {
        private readonly bool _isDevelopment;

        public BugStoreExceptionFilter(bool isDevelopment)
        {
            _isDevelopment = isDevelopment;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            string stackTrace = _isDevelopment ? context.Exception.StackTrace : String.Empty;
            string message = ex.Message;
            string error = String.Empty;

            IActionResult actionResult;
            if(ex is InvalidQuantityException)
            {
                // return a status code 400
                error = "Invalid quantity request.";
                actionResult = new BadRequestObjectResult(CreateContent(error, message, stackTrace));
            }else if (ex is DbUpdateConcurrencyException)
            {
                // return a status code 400
                error = "Concurency Issue";
                actionResult = new BadRequestObjectResult(CreateContent(error, message, stackTrace));
            }
            else
            {
                error = "General error";
                actionResult = new ObjectResult(CreateContent(error, message, stackTrace))
                {
                    StatusCode = 500
                };
            }
            context.Result = actionResult;
        }

        private static object CreateContent(string error, string message, string stackTrace)
        {
            return new
            {
                Error = error,
                Message = message,
                StackTrace = stackTrace
            };
        }
    }
}
