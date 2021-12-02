using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public IActionResult RouteToReact()
        {
            return RedirectPermanent("/");
        }
    }
}
