using Microsoft.AspNetCore.Http;
using PortfolioWeb.Data;
using PortfolioWeb.Models;
using System.Threading.Tasks;

namespace PortfolioWeb.Services
{
    public class VisitorLogger
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VisitorLogger(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogVisit(string page)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = httpContext.Request.Headers["User-Agent"].ToString();

            var log = new VisitorLog
            {
                IPAddress = ipAddress,
                UserAgent = userAgent,
                PageVisited = page
            };

            _context.VisitorLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}