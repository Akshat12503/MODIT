namespace ModitBackend.Models
{
    public class VendorProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int VendorId { get; set; }
        public Vendor Vendor { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQty { get; set; }
        public int MinOrderQty { get; set; } = 1;
        public string? BulkPriceTiersJson { get; set; } // e.g. [{"qty":15,"price":330}]
        public bool IsAvailable { get; set; } = true;
    }
}