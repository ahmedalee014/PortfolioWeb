using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using PortfolioWeb.Services;
using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Areas.Admin.Pages.Projects
{
    [Authorize(Policy = "ManageContent")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public ProjectInputModel Project { get; set; } = new();

        [BindProperty]
        [Display(Name = "Project Image")]
        public IFormFile? ImageFile { get; set; }

        public string? ExistingImagePath { get; set; }

        public EditModel(
            ApplicationDbContext context,
            FileUploadService fileUploadService,
            ILogger<EditModel> logger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            Project = new ProjectInputModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                Technologies = project.Technologies,
                GitHubUrl = project.GitHubUrl,
                LiveDemoUrl = project.LiveDemoUrl,
                IsFeatured = project.IsFeatured
            };

            ExistingImagePath = project.ImagePath;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var projectToUpdate = await _context.Projects.FindAsync(Project.Id);
            if (projectToUpdate == null)
            {
                return NotFound();
            }

            // Update properties
            projectToUpdate.Title = Project.Title;
            projectToUpdate.Description = Project.Description;
            projectToUpdate.Technologies = Project.Technologies;
            projectToUpdate.GitHubUrl = Project.GitHubUrl;
            projectToUpdate.LiveDemoUrl = Project.LiveDemoUrl;
            projectToUpdate.IsFeatured = Project.IsFeatured;

            // Handle image upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(projectToUpdate.ImagePath))
                {
                    _fileUploadService.DeleteFile(projectToUpdate.ImagePath);
                }
                
                // Upload new image
                projectToUpdate.ImagePath = await _fileUploadService.UploadFileAsync(ImageFile);
            }

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Project updated successfully!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ProjectExists(Project.Id))
                {
                    return NotFound();
                }
                _logger.LogError(ex, "Concurrency error updating project ID {ProjectId}", Project.Id);
                ModelState.AddModelError("", "The project was modified by another user. Please refresh and try again.");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating project ID {ProjectId}", Project.Id);
                ModelState.AddModelError("", "An error occurred while updating the project.");
                return Page();
            }
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}