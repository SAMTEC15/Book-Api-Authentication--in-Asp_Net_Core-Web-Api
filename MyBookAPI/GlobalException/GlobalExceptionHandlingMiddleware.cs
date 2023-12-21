using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Common.GlobalException
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate requestDelegate, ILogger logger)
        {
            _requestDelegate = requestDelegate;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch(Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = HttpStatusCode.InternalServerError;
            }
        }
    }
}
