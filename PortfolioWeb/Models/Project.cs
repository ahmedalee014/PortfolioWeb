using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(200)]
        public string Technologies { get; set; }

        [StringLength(200)]
        [Display(Name = "GitHub URL")]
        public string GitHubUrl { get; set; }

        [StringLength(200)]
        [Display(Name = "Live Demo URL")]
        public string LiveDemoUrl { get; set; }

        [StringLength(255)]
        [Display(Name = "Image Path")]
        public string? ImagePath { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsFeatured { get; set; }
        public string? AuthorId { get; set; }
    }
}