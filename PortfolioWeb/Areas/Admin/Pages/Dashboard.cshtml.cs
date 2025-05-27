using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PortfolioWeb.Areas.Admin.Pages
{
    [Authorize(Policy = "AdminOnly")]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}