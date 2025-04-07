using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using System.IO;

namespace ProjetoMvc.Controllers
{
    public class FormsController : Controller
    {
        [HttpPost]
        public IActionResult SubmitFormularioUsuario(string nome, float altura, int idade, float peso, string estado)
        {
            // Cria uma nova planilha Excel
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Dados do Usuário");

                // Adiciona os cabeçalhos na primeira linha
                worksheet.Cell(1, 1).Value = "Nome";
                worksheet.Cell(1, 2).Value = "Altura";
                worksheet.Cell(1, 3).Value = "Idade";
                worksheet.Cell(1, 4).Value = "Peso";
                worksheet.Cell(1, 5).Value = "Estado";

                // Adiciona os valores na segunda linha
                worksheet.Cell(2, 1).Value = nome;
                worksheet.Cell(2, 2).Value = altura;
                worksheet.Cell(2, 3).Value = idade;
                worksheet.Cell(2, 4).Value = peso;
                worksheet.Cell(2, 5).Value = estado;

                // Converte a planilha para um array de bytes
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var excelBytes = stream.ToArray();

                    // Retorna o arquivo Excel para download
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DadosUsuario.xlsx");
                }
            }
        }
    }
}
