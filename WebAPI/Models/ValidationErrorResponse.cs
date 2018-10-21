using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Models
{
    public class ValidationErrorResponse
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationErrorResponse(ModelStateDictionary modelState)
        {
            Errors = modelState.ToDictionary(
                x => x.Key,
                x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            );
        }

        public ValidationErrorResponse(IDictionary<string, string[]> errors)
        {
            Errors = errors;
        }

        public IActionResult ToResult()
        {
            return new BadRequestObjectResult(this);
        }
    }
}
