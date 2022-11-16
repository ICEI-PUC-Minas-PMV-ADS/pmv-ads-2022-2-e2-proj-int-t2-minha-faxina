using Microsoft.AspNetCore.Mvc;

namespace PucWebApplication.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
