using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class BlogPost
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public string Excerpt { get; set; }

        public string Author { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string ImagePath { get; set; }

        public List<string> Tags { get; set; } = new List<string>();
    }
}