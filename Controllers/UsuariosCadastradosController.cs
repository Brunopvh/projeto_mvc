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
    }
}
