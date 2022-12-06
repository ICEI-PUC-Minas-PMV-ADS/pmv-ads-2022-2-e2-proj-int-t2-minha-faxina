using Microsoft.AspNetCore.Mvc;
using PucWebApplication.Data;
using PucWebApplication.Models;
using System.Diagnostics;

namespace PucWebApplication.Controllers {
    public class HomeController : Controller {
        private readonly ILogger<HomeController> _logger;
        private readonly Contexto _context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment hostEnvironment, Contexto _contexto) {
            _logger = logger;
            _context = _contexto;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult EmployeeListing() { 
            return View(_context.Usuario.ToList());
        }

        public ActionResult EmployeeProfileUpload(Usuario empdetails) {
            string uniqueFileName = null;

            if (empdetails.ImageFile != null) {
                string ImageUploadedFolder = Path.Combine
                    (webHostEnvironment.WebRootPath, "UploadedImages");

                uniqueFileName = Guid.NewGuid().ToString() + "_" +
                    empdetails.ImageFile.FileName;

                string filepath = Path.Combine(ImageUploadedFolder, uniqueFileName);

                using (var fileStream = new FileStream(filepath, FileMode.Create)) {
                    empdetails.ImageFile.CopyTo(fileStream);
                }

                empdetails.EmpPhotoPath = "~/wwwroot/UploadedImages";
                empdetails.EmpFileName = uniqueFileName;

                _context.Usuario.Add(empdetails);
                _context.SaveChanges();

                return RedirectToAction("EmployeeListing", "Home");
            }
            return View();
        }

        public IActionResult Index() {           
            
            return View();
        }

        public IActionResult Home() {

            return View(_context.Usuario.ToList());
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}