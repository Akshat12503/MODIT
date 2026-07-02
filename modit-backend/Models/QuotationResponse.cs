namespace ModitBackend.Models
{
    public enum QuotationResponseStatus { Submitted, Accepted, Rejected }

    public class QuotationResponse
    {
        public int Id { get; set; }
        public int QuotationRequestId { get; set; }
        public QuotationRequest QuotationRequest { get; set; } = null!;
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public decimal QuotedPrice { get; set; }
        public string DeliveryTimeEstimate { get; set; } = string.Empty; // e.g. "2 days"
        public string? Notes { get; set; }
        public QuotationResponseStatus Status { get; set; } = QuotationResponseStatus.Submitted;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}