using Microsoft.AspNetCore.Mvc;

namespace ProjetoMvc.Controllers
{
    public class FormsController : Controller
    {
        [HttpPost]
        public IActionResult SubmitFormularioUsuario(string nome, float altura, int idade, float peso, string estado)
        {
            TempData["AlertMessage"] = $"Nome: {nome}, Altura: {altura}, Idade: {idade}, Peso: {peso}, Estado: {estado}";
            return RedirectToAction("Index", "Home");
        }
    }
}
