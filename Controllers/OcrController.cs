using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;
using Tesseract;

namespace ProjetoMvc.Controllers
{
    public class OcrController : Controller
    {
        // Exibe a página para carregar a imagem
        public IActionResult UploadImagem()
        {
            return View();
        }

        // Recebe a imagem, reconhece o texto com OCR e permite o download do PDF gerado
        [HttpPost]
        public IActionResult ProcessarImagem(IFormFile imagemFile)
        {
            if (imagemFile == null || imagemFile.Length == 0)
            {
                ModelState.AddModelError("imagemFile", "Por favor, selecione um arquivo de imagem.");
                return View("~/Views/Ocr/UploadImagem.cshtml");
            }

            string textoReconhecido;
            using (var memoryStream = new MemoryStream())
            {
                // Carrega a imagem no memoryStream
                imagemFile.CopyTo(memoryStream);
                memoryStream.Position = 0;

                // Reconhece o texto da imagem usando Tesseract
                using (var engine = new TesseractEngine(@"./tessdata", "por", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromMemory(memoryStream.ToArray()))
                    {
                        using (var page = engine.Process(img))
                        {
                            textoReconhecido = page.GetText();
                        }
                    }
                }

                // Resetar a posição do memoryStream para carregar a imagem novamente
                memoryStream.Position = 0;

                // Cria um novo documento PDF
                var document = new PdfDocument();
                var pdfPage = document.AddPage();
                var gfx = XGraphics.FromPdfPage(pdfPage);

                // Adiciona a imagem ao PDF
                using (XImage image = XImage.FromStream(() => new MemoryStream(memoryStream.ToArray())))
                {
                    gfx.DrawImage(image, 0, 0, pdfPage.Width, pdfPage.Height);
                }

                // Adiciona o texto reconhecido ao PDF, respeitando as quebras de linha
                var font = new XFont("Verdana", 12, XFontStyle.Regular);
                var lines = textoReconhecido.Split('\n');
                double yPosition = 0;
                foreach (var line in lines)
                {
                    gfx.DrawString(line, font, XBrushes.Black, new XRect(0, yPosition, pdfPage.Width, pdfPage.Height), XStringFormats.TopLeft);
                    yPosition += gfx.MeasureString(line, font).Height;
                }

                // Converte o PDF para um array de bytes
                using (var pdfStream = new MemoryStream())
                {
                    document.Save(pdfStream);
                    var pdfBytes = pdfStream.ToArray();

                    // Retorna o arquivo PDF para download
                    return File(pdfBytes, "application/pdf", "imagem_ocr.pdf");
                }
            }
        }
    }
}
