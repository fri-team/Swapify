using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult RouteToReact()
        {
            return RedirectPermanent("/");
        }
    }
}
