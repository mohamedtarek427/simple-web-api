

using System.Diagnostics;

namespace WebApplication1.Middleware
{
    public class profilingMiddleware 
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<profilingMiddleware> _logger;

        public profilingMiddleware(RequestDelegate next, ILogger<profilingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            await _next(context);
            stopwatch.Stop();
            _logger.LogInformation($"request `{context.Request.Path}` took `${stopwatch.ElapsedMilliseconds}ms`");
        }
    }
}
