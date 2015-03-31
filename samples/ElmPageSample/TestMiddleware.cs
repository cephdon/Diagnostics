using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.Framework.Logging;

namespace ElmPageSample
{
    public class TestMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;

        public TestMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<TestMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            using (_logger.BeginScope("GreetingScope"))
            {
                _logger.LogVerbose("Getting message");

                await httpContext.Response.WriteAsync("Hello World!");
            }
        }
    }
}