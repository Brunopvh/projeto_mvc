// Controllers/UsuarioController.cs
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.Json;
using ProjetoMvc.Models;

namespace ProjetoMvc.Controllers
{
    public class UsuarioController : Controller
    {
        // Exibe o formulário para inserir dados do usuário
        public IActionResult Inserir()
        {
            return View();
        }

        // Ação que processa os dados do usuário e gera o arquivo JSON
        [HttpPost]
        public IActionResult Salvar(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Serializa os dados do usuário em JSON
                var usuarioJson = JsonSerializer.Serialize(usuario);

                // Cria o arquivo JSON em memória (não o salva no servidor)
                var fileBytes = System.Text.Encoding.UTF8.GetBytes(usuarioJson);

                // Retorna o arquivo JSON como download
                return File(fileBytes, "application/json", "data.json");
            }

            // Caso os dados não sejam válidos, retorna para o formulário
            return View("Inserir");
        }
    }
}
