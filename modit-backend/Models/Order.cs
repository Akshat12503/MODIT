namespace ModitBackend.Models
{
    public enum OrderStatus { Placed, Confirmed, Dispatched, Delivered, Cancelled }
    public enum PaymentMode { Prepaid, COD, Credit }

    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Placed;
        public string DeliveryAddress { get; set; } = string.Empty;
        public PaymentMode PaymentMode { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public Invoice? Invoice { get; set; }
    }
}