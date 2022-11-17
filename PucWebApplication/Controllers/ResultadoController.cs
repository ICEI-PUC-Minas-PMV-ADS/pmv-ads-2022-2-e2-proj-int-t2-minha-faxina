using Microsoft.AspNetCore.Mvc;
using PucWebApplication.Data;

namespace PucWebApplication.Controllers
{

    // Controlador da Pasta Resultado //
    public class ResultadoController : Controller
    {
        private readonly Contexto _context;

        public ResultadoController(Contexto context)
        {
            _context = context;
        }
        public IActionResult Pesquisa(string Pesquisas = "")
        {
            var q = _context.Usuario.AsQueryable();
            if (!string.IsNullOrEmpty(Pesquisas))
            {
                q = q.Where(c => c.Bairro.Contains(Pesquisas));
                q = q.OrderBy(c => c.Nome);


                return View(q.ToList());

            }

            return View(_context.Usuario.ToList());
        }

    }

    // Fim Controlador da Pasta Resultado //

}
