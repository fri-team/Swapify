using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;
using WebAPI.Models.ErrorResponseModels;

namespace WebAPI.Filters
{
    public class ValidationFilter : IActionFilter
    {
        private readonly ILogger<ValidationFilter> _logger;

        public ValidationFilter(ILogger<ValidationFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            //No need for OnActionExecuted method
        }

        public void OnActionExecuting(ActionExecutingContext context)
       {
            if (!context.ModelState.IsValid)
            {
                context.Result = new ValidationErrorResponse(context.ModelState).ToResult();
                StringBuilder modelStateBuilder = context.ModelState.Values.SelectMany(x => x.Errors).Aggregate(
                                new StringBuilder($"ModelState errors: "),
                                (sb, x) => sb.Append($"{x.ErrorMessage} "));
                _logger.LogInformation(modelStateBuilder.ToString());
            }
        }
    }
}
