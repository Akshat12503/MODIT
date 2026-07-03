namespace ModitBackend.DTOs
{
    public class RequiredMaterialItem
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
    }

    public class CreateQuotationRequestDto
    {
        public int UserId { get; set; }
        public string ProjectDescription { get; set; } = string.Empty;
        public List<RequiredMaterialItem> RequiredMaterials { get; set; } = new();
        public DateTime Deadline { get; set; }
    }

    public class QuotationRequestDto
    {
        public int Id { get; set; }
        public string ProjectDescription { get; set; } = string.Empty;
        public List<RequiredMaterialItem> RequiredMaterials { get; set; } = new();
        public DateTime Deadline { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<QuotationResponseDto> Responses { get; set; } = new();
    }

    public class SubmitQuotationResponseDto
    {
        public int QuotationRequestId { get; set; }
        public int VendorId { get; set; }
        public decimal QuotedPrice { get; set; }
        public string DeliveryTimeEstimate { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }

    public class QuotationResponseDto
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        public string VendorShopName { get; set; } = string.Empty;
        public double VendorRating { get; set; }
        public decimal QuotedPrice { get; set; }
        public string DeliveryTimeEstimate { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class RespondToQuoteDto
    {
        public int QuotationResponseId { get; set; }
        public bool Accept { get; set; }
    }
}