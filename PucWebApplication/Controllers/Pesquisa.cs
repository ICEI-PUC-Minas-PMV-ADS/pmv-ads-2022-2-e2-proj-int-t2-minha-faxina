using Microsoft.AspNetCore.Mvc;

namespace PucWebApplication.Controllers
{
    public class Pesquisa : Controller
    {
        public IActionResult Resultado()
        {
            return View();
        }
    }
}
