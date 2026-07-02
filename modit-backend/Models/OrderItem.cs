namespace ModitBackend.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int VendorProductId { get; set; }
        public VendorProduct VendorProduct { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; } // snapshot price at time of order
    }
}