using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PortfolioWeb.Pages.Identity
{
    [AllowAnonymous]
    public class AccessDeniedModel : PageModel
    {
        public void OnGet() { }
    }
}