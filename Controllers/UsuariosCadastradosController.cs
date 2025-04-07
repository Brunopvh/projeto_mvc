using Microsoft.AspNetCore.Mvc;
using ProjetoMvc.Models;
using System.Linq;

namespace ProjetoMvc.Controllers
{
    public class UsuariosCadastradosController : Controller
    {
        private readonly AppDbContext _context;

        public UsuariosCadastradosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View("UsuariosCadastrados", usuarios);
        }

        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Update(usuario);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usuario);
        }
    }
}
