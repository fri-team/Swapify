using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using WebAPI.Models.ErrorResponseModels;

namespace WebAPI.Controllers
{
    public class BaseController : Controller
    {
        [NonAction]
        public IActionResult ValidationError(ModelStateDictionary keyValuePairs)
        {
            return new ValidationErrorResponse(keyValuePairs).ToResult();
        }

        [NonAction]
        public IActionResult ValidationError(Dictionary<string, string[]> keyValuePairs)
        {
            return new ValidationErrorResponse(keyValuePairs).ToResult();
        }

        [NonAction]
        public IActionResult ErrorResponse(string error)
        {
            return new ErrorResponse(error).ToResult();
        }
    }
}
