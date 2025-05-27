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
        private readonly ILogger<IndexModel> _logger;

        public List<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();

        public IndexModel(
            ApplicationDbContext context,
            VisitorLogger visitorLogger,
            ILogger<IndexModel> logger)
        {
            _context = context;
            _visitorLogger = visitorLogger;
            _logger = logger;
        }

        public async Task OnGetAsync()
        {
            try
            {
                await _visitorLogger.LogVisit("Blog");

                BlogPosts = await _context.BlogPosts
                    .OrderByDescending(b => b.PublishedDate)
                    .AsNoTracking()
                    .ToListAsync();

                _logger.LogInformation($"Loaded {BlogPosts.Count} blog posts");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading blog posts");
                BlogPosts = new List<BlogPost>
                {
                    new BlogPost
                    {
                        Id = 1,
                        Title = "Sample Post",
                        Excerpt = "This is a sample post that appears when there's an error",
                        Content = "Sample content",
                        ImagePath = "/images/default-blog.jpg",
                        PublishedDate = DateTime.Now
                    }
                };
            }
        }
    }
}