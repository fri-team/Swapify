using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public IActionResult RouteToReact()
        {
            return RedirectPermanent("/");
        }
    }
}
