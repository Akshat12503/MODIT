namespace ModitBackend.Models
{
    public class Vendor
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string ShopName { get; set; } = string.Empty;
        public string ServiceZones { get; set; } = string.Empty; // comma-separated pincodes/areas for now
        public double Rating { get; set; } = 0;
        public double DeliveryRadiusKm { get; set; }
        public bool IsApproved { get; set; } = false;

        public ICollection<VendorProduct> VendorProducts { get; set; } = new List<VendorProduct>();
    }
}