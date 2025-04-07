//using EPPlus.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;

namespace SeuProjeto.Controllers
{
    public class PlanilhaController : Controller
    {
        // Exibe a página para upload da planilha
        public IActionResult ExibirPlanilha()
        {
            return View();
        }

        // Processa o upload e exibe as 3 primeiras linhas da planilha
        [HttpPost]
        public IActionResult ExibirPlanilha(IFormFile arquivoPlanilha)
        {
            if (arquivoPlanilha == null || arquivoPlanilha.Length == 0)
            {
                ModelState.AddModelError("arquivoPlanilha", "Por favor, selecione uma planilha.");
                return View();
            }

            List<List<string>> linhas = new List<List<string>>();
            List<string> cabecalhos = new List<string>();

            using (var stream = new MemoryStream())
            {
                arquivoPlanilha.CopyTo(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0]; // Primeira planilha (0-indexed)
                    var totalLinhas = worksheet.Dimension.Rows;
                    var totalColunas = worksheet.Dimension.Columns;

                    // Lê o cabeçalho (primeira linha)
                    for (int col = 1; col <= totalColunas; col++)
                    {
                        cabecalhos.Add(worksheet.Cells[1, col].Text);
                    }

                    // Lê as 3 primeiras linhas (depois do cabeçalho)
                    for (int i = 2; i <= 4 && i <= totalLinhas; i++)
                    {
                        List<string> linha = new List<string>();
                        for (int j = 1; j <= totalColunas; j++)
                        {
                            linha.Add(worksheet.Cells[i, j].Text);
                        }
                        linhas.Add(linha);
                    }
                }
            }

            // Passa os dados para a view
            ViewData["Cabecalhos"] = cabecalhos;
            ViewData["Linhas"] = linhas;

            return View();
        }
    }
}
