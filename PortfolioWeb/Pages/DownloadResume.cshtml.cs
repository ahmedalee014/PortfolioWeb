using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IO;

namespace PortfolioWeb.Pages
{
    [Authorize] // Requires login
    public class DownloadResumeModel : PageModel
    {
        private readonly ILogger<DownloadResumeModel> _logger;
        private readonly IWebHostEnvironment _env;

        public DownloadResumeModel(ILogger<DownloadResumeModel> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult OnGet()
        {
            var filePath = Path.Combine(_env.WebRootPath, "Ahmed-Ali-Resume.pdf");
            _logger.LogInformation($"Looking for resume at: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                _logger.LogError("Resume file not found at: " + filePath);
                return NotFound();
            }

            _logger.LogInformation("Resume found, initiating download");
            return PhysicalFile(filePath, "application/pdf", "Ahmed-Ali-Resume.pdf");
        }
    }
}