// PortfolioWeb/Models/ProjectInputModel.cs
using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class ProjectInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [StringLength(200)]
        public string Technologies { get; set; } = string.Empty;

        [StringLength(200)]
        [Url]
        [Display(Name = "GitHub URL")]
        public string GitHubUrl { get; set; } = string.Empty;

        [StringLength(200)]
        [Url]
        [Display(Name = "Live Demo URL")]
        public string LiveDemoUrl { get; set; } = string.Empty;

        public bool IsFeatured { get; set; }
    }
}