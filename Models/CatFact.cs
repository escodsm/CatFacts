namespace CatFactsApp.Models
{
    public class CatFact
    {
        public string Text { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LogStatus { get; set; } // Added for UI display
    }
}