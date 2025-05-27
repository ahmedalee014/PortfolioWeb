using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class BlogPostInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Excerpt { get; set; }

        [Required]
        public string Content { get; set; }

        public bool IsPublished { get; set; }
        public DateTime PublishedDate { get; set; }
    }
}

//using System.ComponentModel.DataAnnotations;

//namespace PortfolioWeb.Models
//{
//    // Ensure there is only one definition of BlogPostInputModel in the namespace
//    public class BlogPostInputModel
//    {
//        public int Id { get; set; }

//        [Required]
//        [StringLength(200)]
//        public required string Title { get; set; }

//        [StringLength(500)]
//        public required string Excerpt { get; set; }

//        [Required]
//        public required string Content { get; set; }

//        public bool IsPublished { get; set; }
//        public DateTime PublishedDate { get; set; }
//    }
//}