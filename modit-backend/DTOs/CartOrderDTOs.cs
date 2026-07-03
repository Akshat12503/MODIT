using ModitBackend.Models;

namespace ModitBackend.DTOs
{
    public class AddToCartDto
    {
        public int UserId { get; set; }
        public int VendorProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemDto
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartItemDto
    {
        public int CartItemId { get; set; }
        public int VendorProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string VendorShopName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class CartDto
    {
        public int CartId { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }

    public class PlaceOrderDto
    {
        public int UserId { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public PaymentMode PaymentMode { get; set; }
    }

    public class OrderDto
    {
        public int Id { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DeliveryAddress { get; set; } = string.Empty;
        public string PaymentMode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public string VendorShopName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal PriceAtOrder { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class UpdateOrderStatusDto
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}