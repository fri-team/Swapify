using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.ErrorResponseModels
{
    public class ErrorResponse
    {
        public string Error { get; }

        public ErrorResponse(string error)
        {
            Error = error;
        }

        public IActionResult ToResult()
        {
            return new BadRequestObjectResult(this);
        }
    }
}
