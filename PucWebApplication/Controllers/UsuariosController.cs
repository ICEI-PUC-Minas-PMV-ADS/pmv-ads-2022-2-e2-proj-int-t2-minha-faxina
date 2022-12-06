using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Crypto.Generators;
using PucWebApplication.Data;
using PucWebApplication.Models;

namespace PucWebApplication.Controllers
{

    public class UsuariosController : Controller
    {
        private readonly Contexto _context;
        private string caminhoServidor;
        
        private readonly IWebHostEnvironment webHostEnvironment;


        public UsuariosController(Contexto context, IWebHostEnvironment sistema, IWebHostEnvironment hostEnvironment, Contexto _contexto)
        {
            _context = context;
            caminhoServidor = sistema.WebRootPath;
            webHostEnvironment = hostEnvironment;
        }

        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> login([Bind("Id,Email,Senha")] Usuario usuario)
        {
            var user = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Email == usuario.Email);

            if (user == null)
            {
                ViewBag.Message = "Usuário e/ou Senha inválidos!";
                return View();
            }

            bool isSenhaOk = BCrypt.Net.BCrypt.Verify(usuario.Senha, user.Senha);

            if (isSenhaOk)
            {

                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.Nome),
                    new Claim(ClaimTypes.NameIdentifier, user.Nome),
                    new Claim(ClaimTypes.Role, user.Perfil.ToString())
                };

                var userIdentity = new ClaimsIdentity(claims, "Login");

                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                var props = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTime.Now.ToLocalTime().AddDays(7),
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(principal, props);


                return Redirect("/Home");
            }

            ViewBag.Message = "Usuário e/ou Senha inválidos!";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        //Subir Imagens do Usuário
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile foto)
        {
            string caminhoParaSalvaImagem = caminhoServidor + "\\ImagensUsuarioUpload\\";
            string novoNomeParaImage = Guid.NewGuid().ToString() + "__________" + foto.FileName;
            if (!Directory.Exists(caminhoParaSalvaImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvaImagem);
            }

            using (var stream = System.IO.File.Create(caminhoParaSalvaImagem + novoNomeParaImage))
            {
                foto.CopyToAsync(stream);
            }
            return RedirectToAction("Upload");
        }

        // GET: Usuarios
        public IActionResult Index(string Pesquisa = "")
        {
            var q = _context.Usuario.AsQueryable();
            if (!string.IsNullOrEmpty(Pesquisa))
            {
                q = q.Where(c => c.Bairro.Contains(Pesquisa));
                q = q.OrderBy(c => c.Nome);


                return View(q.ToList());

            }

            return View(_context.Usuario.ToList());
        }

        //backup
        //public async Task<IActionResult> Index() {
        //    return View(await _context.Usuario.ToListAsync());
        //}

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        public IActionResult MeuPerfil()
        {
            return View();
        }

        // GET: Usuarios/Create

        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,cpf,Idade,cep,Rua,Numero,Complemento,Bairro,Cidade,Senha,Perfil")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(login));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Nome == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,cpf,Idade,cep,Rua,Numero,Complemento,Bairro,Cidade,Senha,Perfil")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuario == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuario == null)
            {
                return Problem("Entity set 'Contexto.Usuario'  is null.");
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuario.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }

        // GET: novo cadastro
        public IActionResult Cadastro()
        {
            return View();
        }

        // POST: UsuarioUploadImg/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastro([Bind("Id,Nome,Email,cpf,Idade,cep,Rua,Numero,Complemento,Bairro,Cidade,Senha,Perfil,tel,EmpPhotoPath,EmpFileName")] Usuario usuario, Usuario empdetails)
        {       


            if (ModelState.IsValid)
            {
                usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha);                
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(login));

            }            


            return View(usuario);
            //if (empdetails.ImageFile != null) {
            //    string ImageUploadedFolder = Path.Combine
            //        (webHostEnvironment.WebRootPath, "UploadedImages");

            //    uniqueFileName = Guid.NewGuid().ToString() + "_" +
            //        empdetails.ImageFile.FileName;

            //    string filepath = Path.Combine(ImageUploadedFolder, uniqueFileName);

            //    using (var fileStream = new FileStream(filepath, FileMode.Create)) {
            //        empdetails.ImageFile.CopyTo(fileStream);
            //    }

            //    empdetails.EmpPhotoPath = "~/wwwroot/UploadedImages";
            //    empdetails.EmpFileName = uniqueFileName;

            //    _context.Usuario.Add(empdetails);
            //    _context.SaveChanges();

            //    return RedirectToAction("Index", "Home");
            //}            

        }
    }
}
