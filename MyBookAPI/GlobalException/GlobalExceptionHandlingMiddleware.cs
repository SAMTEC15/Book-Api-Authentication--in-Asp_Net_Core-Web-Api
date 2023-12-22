using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MyBook.Common.GlobalException
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {            
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var problem = new ProblemDetails()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Type = "Server error",
                    Title = "Internal Server error",
                    Detail = "An internal server Server has occurred, we currently looking at fixing this."
                };
                var json = JsonSerializer.Serialize(problem);
                await context.Response.WriteAsync(json);
                context.Response.ContentType = "application/json";
            }
        }
    }
}
