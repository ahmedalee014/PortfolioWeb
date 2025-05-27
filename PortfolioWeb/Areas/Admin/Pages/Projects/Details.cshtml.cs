using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Data;
using PortfolioWeb.Models;

namespace PortfolioWeb.Areas.Admin.Pages.Projects
{
    [Authorize(Policy = "ManageContent")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Project Project { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Project = await _context.Projects.FirstOrDefaultAsync(m => m.Id == id);

            if (Project == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}