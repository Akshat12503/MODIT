namespace ModitBackend.DTOs
{
    public class RegisterVendorDto
    {
        public int UserId { get; set; } // must already be a registered User with role=Supplier
        public string ShopName { get; set; } = string.Empty;
        public string ServiceZones { get; set; } = string.Empty; // comma-separated area names/pincodes
        public double DeliveryRadiusKm { get; set; }
    }

    public class VendorDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ShopName { get; set; } = string.Empty;
        public string ServiceZones { get; set; } = string.Empty;
        public double Rating { get; set; }
        public double DeliveryRadiusKm { get; set; }
        public bool IsApproved { get; set; }
    }

    public class ApproveVendorDto
    {
        public int VendorId { get; set; }
        public bool Approve { get; set; }
    }
}