using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using PortfolioWeb.Services;

namespace PortfolioWeb.Areas.Admin.Pages.Projects
{
    [Authorize(Policy = "ManageContent")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ApplicationDbContext context,
            FileUploadService fileUploadService,
            ILogger<IndexModel> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public IList<Project> Projects { get; set; } = new List<Project>();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            Projects = await _context.Projects
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                var project = await _context.Projects.FindAsync(id);
                if (project == null)
                {
                    ErrorMessage = "Project not found";
                    return RedirectToPage();
                }

                // Delete associated image file if exists
                if (!string.IsNullOrEmpty(project.ImagePath))
                {
                    _fileUploadService.DeleteFile(project.ImagePath);
                }

                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();

                SuccessMessage = "Project deleted successfully";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error deleting project ID {ProjectId}", id);
                ErrorMessage = "An error occurred while deleting the project. Please try again.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error deleting project ID {ProjectId}", id);
                ErrorMessage = "An unexpected error occurred. Please try again.";
            }

            return RedirectToPage();
        }
    }
}