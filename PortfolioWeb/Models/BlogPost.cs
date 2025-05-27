using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(255)]
        public string? ImagePath { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
        public bool IsPublished { get; set; } = true;

        // Foreign key to IdentityUser
        public string AuthorId { get; set; }

        // Navigation property
        public virtual IdentityUser Author { get; set; }
    }
}