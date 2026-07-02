namespace ModitBackend.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        public int VendorProductId { get; set; }
        public VendorProduct VendorProduct { get; set; } = null!;
        public int Quantity { get; set; }
    }
}