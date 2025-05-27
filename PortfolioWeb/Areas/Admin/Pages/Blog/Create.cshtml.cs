using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using PortfolioWeb.Services;
using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Areas.Admin.Pages.Blog
{
    [Authorize(Policy = "ManageContent")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FileUploadService _fileUploadService;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public BlogPostInputModel BlogPost { get; set; } = new BlogPostInputModel();

        [BindProperty]
        [Display(Name = "Blog Image")]
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

        public void OnGet()
        {
            BlogPost.PublishedDate = DateTime.UtcNow;
            BlogPost.IsPublished = true;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
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

                var blogPost = new BlogPost
                {
                    Title = BlogPost.Title,
                    Excerpt = BlogPost.Excerpt,
                    Content = BlogPost.Content,
                    IsPublished = BlogPost.IsPublished,
                    AuthorId = user.Id, // Set the AuthorId
                    PublishedDate = BlogPost.PublishedDate
                };

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    blogPost.ImagePath = await _fileUploadService.UploadFileAsync(ImageFile);
                }

                _context.BlogPosts.Add(blogPost);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Blog post created successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating blog post");
                ModelState.AddModelError("", "An error occurred while creating the post.");
                
                return Page();
            }
        }
    }
}