using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class Experience
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Company { get; set; }

        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public bool IsCurrent { get; set; }

        public string Description { get; set; }
    }
}