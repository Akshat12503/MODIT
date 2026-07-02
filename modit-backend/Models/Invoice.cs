namespace ModitBackend.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public decimal GstAmount { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public string? InvoiceUrl { get; set; }
        public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    }
}