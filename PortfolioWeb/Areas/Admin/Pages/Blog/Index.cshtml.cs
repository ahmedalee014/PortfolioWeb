using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PortfolioWeb.Areas.Admin.Pages.Blog
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BlogPost> BlogPosts { get; set; }

        public async Task OnGetAsync()
        {
            BlogPosts = await _context.BlogPosts
                .Include(b => b.Author) // Now this will work
                .OrderByDescending(b => b.PublishedDate)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            // Delete associated image file if exists
            if (!string.IsNullOrEmpty(post.ImagePath))
            {
                var filePath = Path.Combine("wwwroot", post.ImagePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Blog post deleted successfully!";
            return RedirectToPage();
        }
    }
}