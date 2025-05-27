using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWeb.Services;
using PortfolioWeb.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Models;

namespace PortfolioWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly VisitorLogger _visitorLogger;
        private readonly ApplicationDbContext _context;

        public IndexModel(VisitorLogger visitorLogger, ApplicationDbContext context)
        {
            _visitorLogger = visitorLogger;
            _context = context;
        }

        public List<PageProject> Projects { get; set; }
        public List<PageBlogPost> BlogPosts { get; set; }

        public async Task OnGetAsync()
        {
            await _visitorLogger.LogVisit("Home");

            // Fetch and map projects
            var dbProjects = await _context.Projects
                .Where(p => p.IsFeatured)
                .OrderByDescending(p => p.CreatedDate)
                .Take(4)
                .ToListAsync();

            Projects = dbProjects.Select(p => new PageProject
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                ImageUrl = p.ImagePath,
                LiveUrl = p.LiveDemoUrl,
                Technologies = p.Technologies,
                Category = p.Technologies,
                IsFeatured = p.IsFeatured,
                CreatedDate = p.CreatedDate
            }).ToList();

            // Fetch and map blog posts
            var dbBlogPosts = await _context.BlogPosts
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.PublishedDate)
                .Take(3)
                .ToListAsync();

            BlogPosts = dbBlogPosts.Select(b => new PageBlogPost
            {
                Id = b.Id,
                Title = b.Title,
                Excerpt = b.Excerpt,
                ImageUrl = b.ImagePath,
                Category = b.Excerpt,
                //ReadTime = b.ReadTime,
                IsPublished = b.IsPublished,
                PublishedDate = b.PublishedDate
            }).ToList();
        }
    }

    public class PageProject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string LiveUrl { get; set; }
        public string Technologies { get; set; }
        public string Category { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class PageBlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public int ReadTime { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}