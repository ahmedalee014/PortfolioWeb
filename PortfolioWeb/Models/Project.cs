using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Technologies { get; set; }

        public string GitHubUrl { get; set; }

        public string LiveDemoUrl { get; set; }

        public string ImagePath { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}