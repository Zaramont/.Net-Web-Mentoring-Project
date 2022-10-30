using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace MyCatalogSite.Filters
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger logger;
        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            this.logger = logger;
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation($"Action {context.RouteData.Values["action"]} on controller {context.RouteData.Values["controller"]} is started.");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation($"Action {context.RouteData.Values["action"]} on controller {context.RouteData.Values["controller"]} is ended.");
        }
    }
}
