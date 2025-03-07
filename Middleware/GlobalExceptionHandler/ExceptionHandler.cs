using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;

namespace Middleware.GlobalExceptionHandler
{
    public static class ExceptionHandler
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public static string HandleException(Exception ex, out object errorResponse)
        {
            logger.Error(ex, "An Error ocurred in the application");

            errorResponse = new
            {
                Success = false,
                Message = "An error occurred",
                ErrorEventArgs = ex.Message
            };

            return JsonConvert.SerializeObject(errorResponse);
        }
    public static object CreateErrorResponse(Exception ex)
        {
            logger.Error(ex, "An Error ocurred in the application");
            return new
            {
                Success = false,
                Message = "An Error ocurred",
                Error = ex.Message
            };
        }

    }
}