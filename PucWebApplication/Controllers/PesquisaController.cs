using Microsoft.AspNetCore.Mvc;

namespace PucWebApplication.Controllers
{
    public class PesquisaController : Controller
    {
        public IActionResult Pesquisa()
        {
            return View();
        }
    }
}
