using System.ComponentModel.DataAnnotations;

namespace PortfolioWeb.Models
{
    public class Education
    {
        public int Id { get; set; }

        [Required]
        public string Institution { get; set; }

        public string Degree { get; set; }

        public string FieldOfStudy { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public string Description { get; set; }
    }
}