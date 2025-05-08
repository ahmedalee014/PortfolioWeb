using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWeb.Services;

namespace PortfolioWeb.Pages
{
    public class AboutModel : PageModel
    {
        private readonly VisitorLogger _visitorLogger;

        public AboutModel(VisitorLogger visitorLogger)
        {
            _visitorLogger = visitorLogger;
        }

        public async Task OnGetAsync()
        {
            await _visitorLogger.LogVisit("About");
        }
    }
}