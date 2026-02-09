namespace TaskManager.Infrastructure.ExternalServices.Models
{
    public class QuoteResponse
    {
        public string _id { get; set; } = string.Empty;
        public string content { get; set; } = string.Empty;
        public string author { get; set; } = string.Empty;
        public string[] tags { get; set; } = Array.Empty<string>();
        public string authorSlug { get; set; } = string.Empty;
        public int length { get; set; }
        public string dateAdded { get; set; } = string.Empty;
        public string dateModified { get; set; } = string.Empty;
    }
}