namespace PortfolioWeb.Models
{
    public class VisitorLog
    {
        public int Id { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
        public string PageVisited { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.Now;
    }
}