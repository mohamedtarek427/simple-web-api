using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace WebApplication1.Filters
{
    public class logActivityFilter : IActionFilter, IAsyncActionFilter
    {
        private ILogger<logActivityFilter> _logger;

        public logActivityFilter(ILogger<logActivityFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // throw new NotImplementedException();
            _logger.LogInformation($"executiong action {context.ActionDescriptor.DisplayName} on controller {context.Controller}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
          //  throw new NotImplementedException();
          _logger.LogInformation($"action {context.ActionDescriptor.DisplayName} fiished excution o controller {context.Controller}");
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
         _logger.LogInformation($"action async {context.ActionDescriptor.DisplayName} fiished excution o controller {context.Controller}");
           await next();
          _logger.LogInformation($"action async {context.ActionDescriptor.DisplayName} fiished excution o controller {context.Controller}");
        }
    }
}
