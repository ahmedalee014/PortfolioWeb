using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWeb.Services;

namespace PortfolioWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly VisitorLogger _visitorLogger;

        public IndexModel(VisitorLogger visitorLogger)
        {
            _visitorLogger = visitorLogger;
        }

        public async Task OnGetAsync()
        {
            await _visitorLogger.LogVisit("Home");
        }
    }
}