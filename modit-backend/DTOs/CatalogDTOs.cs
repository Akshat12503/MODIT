namespace ModitBackend.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
    }

    public class CreateCategoryDto
    {
        public string Name { get; set; } = string.Empty;
        public int? ParentCategoryId { get; set; }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? BrandName { get; set; }
        public List<VendorOfferDto> VendorOffers { get; set; } = new();
    }

    public class VendorOfferDto
    {
        public int VendorProductId { get; set; }
        public int VendorId { get; set; }
        public string VendorShopName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int MinOrderQty { get; set; }
        public bool IsAvailable { get; set; }
        public double VendorRating { get; set; }
    }

    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? BrandName { get; set; }
    }

    public class AddVendorProductDto
    {
        public int ProductId { get; set; }
        public int VendorId { get; set; }
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int MinOrderQty { get; set; } = 1;
        public string? BulkPriceTiersJson { get; set; }
    }
}