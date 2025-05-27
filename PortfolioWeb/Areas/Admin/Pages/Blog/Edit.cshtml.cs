using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using PortfolioWeb.Services;
using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Areas.Admin.Pages.Blog
{
    [Authorize(Policy = "ManageContent")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly FileUploadService _fileUploadService;
        private readonly ILogger<EditModel> _logger;

        [BindProperty]
        public BlogPostInputModel BlogPost { get; set; }

        [BindProperty]
        [Display(Name = "New Image (optional)")]
        public IFormFile? NewImageFile { get; set; }

        public string? CurrentImagePath { get; set; }

        public EditModel(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            FileUploadService fileUploadService,
            ILogger<EditModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null)
            {
                return NotFound();
            }

            BlogPost = new BlogPostInputModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Excerpt = blogPost.Excerpt,
                Content = blogPost.Content,
                IsPublished = blogPost.IsPublished,
                PublishedDate = blogPost.PublishedDate
            };

            CurrentImagePath = blogPost.ImagePath;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var existingPost = await _context.BlogPosts.FindAsync(BlogPost.Id);
                if (existingPost == null)
                {
                    return NotFound();
                }

                existingPost.Title = BlogPost.Title;
                existingPost.Excerpt = BlogPost.Excerpt;
                existingPost.Content = BlogPost.Content;
                existingPost.IsPublished = BlogPost.IsPublished;

                if (NewImageFile != null && NewImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingPost.ImagePath))
                    {
                        _fileUploadService.DeleteFile(existingPost.ImagePath);
                    }
                    existingPost.ImagePath = await _fileUploadService.UploadFileAsync(NewImageFile);
                }

                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Blog post updated successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating blog post");
                ModelState.AddModelError("", "An error occurred while updating the post.");
                return Page();
            }
        }
    }
}