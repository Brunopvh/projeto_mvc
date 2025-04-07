// Controllers/PdfController.cs
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;


namespace ProjetoMvc.Controllers
{
    public class PdfController : Controller
    {
        // Exibe a página para carregar o PDF
        public IActionResult Carregar()
        {
            return View();
        }

        // Recebe o PDF, divide em páginas e permite o download dos arquivos gerados
        [HttpPost]
        public IActionResult DividirPdf(IFormFile pdfFile)
        {
            if (pdfFile == null || pdfFile.Length == 0)
            {
                ModelState.AddModelError("pdfFile", "Por favor, selecione um arquivo PDF.");
                return View("~/Views/Pdf/Carregar.cshtml");
            }

            // Cria uma lista para armazenar os arquivos gerados
            var pdfFiles = new List<byte[]>();

            // Processa o arquivo PDF
            using (var memoryStream = new MemoryStream())
            {
                // Carrega o arquivo PDF no memoryStream
                pdfFile.CopyTo(memoryStream);
                memoryStream.Position = 0;

                // Carrega o PDF usando PdfSharpCore
                var document = PdfReader.Open(memoryStream, PdfDocumentOpenMode.Import);

                // Divida o PDF em páginas separadas
                for (int i = 0; i < document.PageCount; i++)
                {
                    var newDocument = new PdfDocument();
                    newDocument.AddPage(document.Pages[i]);

                    using (var pageMemoryStream = new MemoryStream())
                    {
                        newDocument.Save(pageMemoryStream);
                        pdfFiles.Add(pageMemoryStream.ToArray());
                    }
                }
            }

            // Prepara os arquivos PDF para o download
            var zipStream = new MemoryStream();
            using (var zipArchive = new System.IO.Compression.ZipArchive(zipStream, System.IO.Compression.ZipArchiveMode.Create, true))
            {
                for (int i = 0; i < pdfFiles.Count; i++)
                {
                    var entry = zipArchive.CreateEntry($"pagina_{i + 1}.pdf");
                    using (var entryStream = entry.Open())
                    {
                        entryStream.Write(pdfFiles[i], 0, pdfFiles[i].Length);
                    }
                }
            }

            // Prepara o arquivo ZIP com os PDFs para o download
            zipStream.Position = 0;
            return File(zipStream.ToArray(), "application/zip", "pdf_dividido.zip");
        }
    }
}
