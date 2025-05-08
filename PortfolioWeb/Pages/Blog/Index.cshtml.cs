using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using PortfolioWeb.Services;

namespace PortfolioWeb.Pages.Blog
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly VisitorLogger _visitorLogger;

        public IndexModel(ApplicationDbContext context, VisitorLogger visitorLogger)
        {
            _context = context;
            _visitorLogger = visitorLogger;
        }

        public List<BlogPost> BlogPosts { get; set; }

        public async Task OnGetAsync()
        {
            await _visitorLogger.LogVisit("Blog");
            BlogPosts = await _context.BlogPosts.OrderByDescending(b => b.PublishedDate).ToListAsync();
        }
    }
}