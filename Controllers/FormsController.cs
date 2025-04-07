using Microsoft.AspNetCore.Mvc;
using ProjetoMvc.Models;
using System.Linq;

namespace ProjetoMvc.Controllers
{
    public class FormsController : Controller
    {
        private readonly AppDbContext _context;

        public FormsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult SubmitFormularioUsuario(string nome, float altura, int idade, float peso, string estado)
        {
            // Verificar se o usuário já existe
            var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Nome == nome);
            if (usuarioExistente != null)
            {
                TempData["ErrorMessage"] = "Usuário já existe.";
                return RedirectToAction("Index", "Home");
            }

            // Adicionar novo usuário
            var usuario = new Usuario
            {
                Nome = nome,
                Altura = altura,
                Idade = idade,
                Peso = peso,
                Estado = estado
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Dados do formulário salvos com sucesso.";
            return RedirectToAction("Index", "Home");
        }
    }
}
