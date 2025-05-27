using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace PortfolioWeb.Pages
{
    public class DownloadResumeModel : PageModel
    {
        private readonly ILogger<DownloadResumeModel> _logger;

        public DownloadResumeModel(ILogger<DownloadResumeModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Ahmed-Ali-Resume.pdf");
            _logger.LogInformation($"Looking for resume at: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError("Resume file not found");
                return NotFound();
            }

            _logger.LogInformation("Resume file found, initiating download");
            return PhysicalFile(filePath, "application/pdf", "Ahmed-Ali-Resume.pdf");
        }
    }
}