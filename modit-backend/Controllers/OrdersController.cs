using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModitBackend.Data;
using ModitBackend.DTOs;
using ModitBackend.Helpers;
using ModitBackend.Models;

namespace ModitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Places order from whatever is currently in the user's cart
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderDto dto)
        {
            var cart = await _db.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.VendorProduct)
                .FirstOrDefaultAsync(c => c.UserId == dto.UserId);

            if (cart == null || cart.CartItems.Count == 0)
                return BadRequest(new { message = "Cart is empty." });

            // Credit payment mode requires sufficient credit limit
            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null) return BadRequest(new { message = "User not found." });

            decimal totalAmount = cart.CartItems.Sum(ci => ci.VendorProduct.Price * ci.Quantity);

            if (dto.PaymentMode == PaymentMode.Credit)
            {
                var currentDebt = await _db.CreditLedgers
                    .Where(cl => cl.UserId == dto.UserId && !cl.IsSettled)
                    .SumAsync(cl => cl.TransactionType == TransactionType.Debit ? cl.Amount : -cl.Amount);

                if (currentDebt + totalAmount > user.CreditLimit)
                    return BadRequest(new { message = "Order exceeds available credit limit." });
            }

            var order = new Order
            {
                UserId = dto.UserId,
                TotalAmount = totalAmount,
                Status = OrderStatus.Placed,
                DeliveryAddress = dto.DeliveryAddress,
                PaymentMode = dto.PaymentMode,
                CreatedAt = DateTime.UtcNow
            };

            _db.Orders.Add(order);
            await _db.SaveChangesAsync();

            foreach (var ci in cart.CartItems)
            {
                _db.OrderItems.Add(new OrderItem
                {
                    OrderId = order.Id,
                    VendorProductId = ci.VendorProductId,
                    Quantity = ci.Quantity,
                    PriceAtOrder = ci.VendorProduct.Price
                });

                // Reduce stock
                ci.VendorProduct.StockQty -= ci.Quantity;
                if (ci.VendorProduct.StockQty <= 0)
                    ci.VendorProduct.IsAvailable = false;
            }

            // Generate GST invoice
            var gstAmount = GstInvoiceGenerator.CalculateGst(totalAmount);
            var invoice = new Invoice
            {
                OrderId = order.Id,
                GstAmount = gstAmount,
                InvoiceNumber = GstInvoiceGenerator.GenerateInvoiceNumber(order.Id),
                IssuedAt = DateTime.UtcNow
            };
            _db.Invoices.Add(invoice);

            // If Credit payment, log to ledger
            if (dto.PaymentMode == PaymentMode.Credit)
            {
                _db.CreditLedgers.Add(new CreditLedger
                {
                    UserId = dto.UserId,
                    TransactionType = TransactionType.Debit,
                    Amount = totalAmount,
                    Balance = totalAmount,
                    DueDate = DateTime.UtcNow.AddDays(30),
                    IsSettled = false,
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Clear cart
            _db.CartItems.RemoveRange(cart.CartItems);

            await _db.SaveChangesAsync();

            return Ok(new { orderId = order.Id, totalAmount, gstAmount, invoiceNumber = invoice.InvoiceNumber });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(int userId)
        {
            var orders = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.VendorProduct)
                        .ThenInclude(vp => vp.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.VendorProduct)
                        .ThenInclude(vp => vp.Vendor)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            return Ok(orders.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _db.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.VendorProduct)
                        .ThenInclude(vp => vp.Product)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.VendorProduct)
                        .ThenInclude(vp => vp.Vendor)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return Ok(MapToDto(order));
        }

        // Vendor/admin updates delivery status -> powers real-time tracking (#7)
        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus(UpdateOrderStatusDto dto)
        {
            var order = await _db.Orders.FindAsync(dto.OrderId);
            if (order == null) return NotFound();

            order.Status = dto.Status;
            await _db.SaveChangesAsync();

            return Ok(new { order.Id, order.Status });
        }

        private static OrderDto MapToDto(Order o)
        {
            return new OrderDto
            {
                Id = o.Id,
                TotalAmount = o.TotalAmount,
                Status = o.Status.ToString(),
                DeliveryAddress = o.DeliveryAddress,
                PaymentMode = o.PaymentMode.ToString(),
                CreatedAt = o.CreatedAt,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = oi.VendorProduct.Product.Name,
                    VendorShopName = oi.VendorProduct.Vendor.ShopName,
                    Quantity = oi.Quantity,
                    PriceAtOrder = oi.PriceAtOrder,
                    Subtotal = oi.PriceAtOrder * oi.Quantity
                }).ToList()
            };
        }
    }
}