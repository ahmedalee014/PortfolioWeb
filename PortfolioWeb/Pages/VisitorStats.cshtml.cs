using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using System.Linq;

namespace PortfolioWeb.Pages
{
    [Authorize]
    public class VisitorStatsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VisitorStatsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int TotalVisitors { get; set; }
        public int UniqueVisitors { get; set; }
        public List<PageStat> PageStats { get; set; }

        public class PageStat
        {
            public string Page { get; set; }
            public int Count { get; set; }
        }

        public async Task OnGetAsync()
        {
            TotalVisitors = await _context.VisitorLogs.CountAsync();
            UniqueVisitors = await _context.VisitorLogs.Select(v => v.IPAddress).Distinct().CountAsync();

            PageStats = await _context.VisitorLogs
                .GroupBy(v => v.PageVisited)
                .Select(g => new PageStat
                {
                    Page = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(s => s.Count)
                .ToListAsync();
        }
    }
}