using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebAPI.Models;
using System.Collections.Generic;

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
    }
}
