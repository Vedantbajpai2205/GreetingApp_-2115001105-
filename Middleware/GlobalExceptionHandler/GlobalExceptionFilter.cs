using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

namespace Middleware.GlobalExceptionHandler
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public GlobalExceptionFilter() { }

        public override void OnException(ExceptionContext context)
        {
            var errorResponse = ExceptionHandler.CreateErrorResponse(context.Exception);

            context.Result = new ObjectResult(errorResponse)
            {
                StatusCode = 500 
            };

            context.ExceptionHandled = true;
        }
    }
}
