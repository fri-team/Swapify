using Microsoft.AspNetCore.Mvc;

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
