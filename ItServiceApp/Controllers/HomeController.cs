using Microsoft.AspNetCore.Mvc;

namespace ItServiceApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
