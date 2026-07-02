namespace ModitBackend.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public string Unit { get; set; } = string.Empty; // bag, kg, sqft, piece
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? BrandName { get; set; }

        public ICollection<VendorProduct> VendorProducts { get; set; } = new List<VendorProduct>();
    }
}