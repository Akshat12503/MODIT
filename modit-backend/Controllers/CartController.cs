using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModitBackend.Data;
using ModitBackend.DTOs;
using ModitBackend.Models;

namespace ModitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CartController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /api/cart/user/5
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            var cart = await _db.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.VendorProduct)
                        .ThenInclude(vp => vp.Product)
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.VendorProduct)
                        .ThenInclude(vp => vp.Vendor)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound(new { message = "Cart not found for this user." });

            return Ok(MapToDto(cart));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == dto.UserId);
            if (cart == null) return BadRequest(new { message = "Cart not found for this user." });

            var vendorProduct = await _db.VendorProducts.FindAsync(dto.VendorProductId);
            if (vendorProduct == null) return BadRequest(new { message = "Invalid vendor product." });

            if (dto.Quantity < vendorProduct.MinOrderQty)
                return BadRequest(new { message = $"Minimum order quantity is {vendorProduct.MinOrderQty}." });

            if (dto.Quantity > vendorProduct.StockQty)
                return BadRequest(new { message = "Requested quantity exceeds available stock." });

            var existingItem = await _db.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == cart.Id && ci.VendorProductId == dto.VendorProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                _db.CartItems.Add(new CartItem
                {
                    CartId = cart.Id,
                    VendorProductId = dto.VendorProductId,
                    Quantity = dto.Quantity
                });
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Added to cart." });
        }

        [HttpPut("update-item")]
        public async Task<IActionResult> UpdateItem(UpdateCartItemDto dto)
        {
            var item = await _db.CartItems.FindAsync(dto.CartItemId);
            if (item == null) return NotFound();

            if (dto.Quantity <= 0)
            {
                _db.CartItems.Remove(item);
            }
            else
            {
                item.Quantity = dto.Quantity;
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Cart updated." });
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            var item = await _db.CartItems.FindAsync(cartItemId);
            if (item == null) return NotFound();

            _db.CartItems.Remove(item);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Item removed." });
        }

        private static CartDto MapToDto(Cart cart)
        {
            var items = cart.CartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.Id,
                VendorProductId = ci.VendorProductId,
                ProductName = ci.VendorProduct.Product.Name,
                VendorShopName = ci.VendorProduct.Vendor.ShopName,
                Price = ci.VendorProduct.Price,
                Quantity = ci.Quantity,
                Subtotal = ci.VendorProduct.Price * ci.Quantity
            }).ToList();

            return new CartDto
            {
                CartId = cart.Id,
                Items = items,
                TotalAmount = items.Sum(i => i.Subtotal)
            };
        }
    }
}