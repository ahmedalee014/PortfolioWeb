using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FileUploadService _fileUploadService;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public ProjectInputModel Project { get; set; } = new ProjectInputModel();

        [BindProperty]
        [Display(Name = "Project Image")]
        public IFormFile? ImageFile { get; set; }

        public CreateModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            FileUploadService fileUploadService,
            ILogger<CreateModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Initialize default values
            Project.IsFeatured = false;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid. Errors: {@Errors}",
                    ModelState.Values.SelectMany(v => v.Errors));
                return Page();
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found");
                    return Page();
                }

                // Create new Project entity
                var project = new Project
                {
                    Title = Project.Title,
                    Description = Project.Description,
                    Technologies = Project.Technologies,
                    GitHubUrl = Project.GitHubUrl,
                    LiveDemoUrl = Project.LiveDemoUrl,
                    IsFeatured = Project.IsFeatured,
                    AuthorId = user.Id,
                    CreatedDate = DateTime.UtcNow
                };

                // Handle image upload if provided
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    project.ImagePath = await _fileUploadService.UploadFileAsync(ImageFile);
                }

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Project created successfully!";
                return RedirectToPage("./Index");
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating project");
                ModelState.AddModelError("", "A database error occurred while saving. Please try again.");
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error creating project");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return Page();
            }
        }
    }
}