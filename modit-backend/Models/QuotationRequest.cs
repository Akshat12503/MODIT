namespace ModitBackend.Models
{
    public enum QuotationRequestStatus { Open, Closed, Fulfilled }

    public class QuotationRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string ProjectDescription { get; set; } = string.Empty;
        public string RequiredMaterialsJson { get; set; } = string.Empty; // [{"product":"Cement","qty":100,"unit":"bag"}]
        public DateTime Deadline { get; set; }
        public QuotationRequestStatus Status { get; set; } = QuotationRequestStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<QuotationResponse> Responses { get; set; } = new List<QuotationResponse>();
    }
}