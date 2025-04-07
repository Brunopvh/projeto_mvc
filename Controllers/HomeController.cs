using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProjetoMvc.Models;

namespace ProjetoMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        
        public IActionResult Index()
        {
            // Abrir HomePage
            return View();
            // Redireciona para o controlador Usuario e a ação Inserir
            //return RedirectToAction("Inserir", "Usuario");
            //return RedirectToAction("Carregar", "Pdf");
            //return RedirectToAction("ExibirPlanilha", "Planilha");
        }
        public IActionResult Carregar()
        {
            return View("~/Views/Pdf/Carregar.cshtml");
        }

        public IActionResult ExibirPlanilha()
        {
            return View("~/Views/Planilha/ExibirPlanilha.cshtml");
        }

        public IActionResult FormularioUsuario()
        {
            return View("~/Views/Forms/FormularioUsuario.cshtml");
        }

        public IActionResult UsuariosCadastrados()
        {
            return RedirectToAction("Index", "UsuariosCadastrados");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
