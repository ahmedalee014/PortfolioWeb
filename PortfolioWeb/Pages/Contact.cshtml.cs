using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioWeb.Models;
using PortfolioWeb.Services;
using PortfolioWeb.Data;

namespace PortfolioWeb.Pages
{
    public class ContactModel : PageModel
    {
        private readonly EmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly VisitorLogger _visitorLogger;

        public ContactModel(EmailService emailService, ApplicationDbContext context, VisitorLogger visitorLogger)
        {
            _emailService = emailService;
            _context = context;
            _visitorLogger = visitorLogger;
        }

        [BindProperty]
        public ContactForm ContactForm { get; set; }

        public async Task OnGetAsync()
        {
            await _visitorLogger.LogVisit("Contact");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Save to database
            _context.ContactForms.Add(ContactForm);
            await _context.SaveChangesAsync();

            // Send email
            var subject = $"New Contact Form Submission: {ContactForm.Subject}";
            var body = $"Name: {ContactForm.Name}<br>Email: {ContactForm.Email}<br>Message: {ContactForm.Message}";

            await _emailService.SendEmailAsync("your.email@example.com", subject, body);

            TempData["Message"] = "Your message has been sent successfully!";
            return RedirectToPage("/Contact");
        }
    }
}