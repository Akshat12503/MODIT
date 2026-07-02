using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModitBackend.Data;
using ModitBackend.DTOs;
using ModitBackend.Models;

namespace ModitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET /api/products?categoryId=2&search=cement
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? categoryId, [FromQuery] string? search)
        {
            var query = _db.Products
                .Include(p => p.Category)
                .Include(p => p.VendorProducts)
                    .ThenInclude(vp => vp.Vendor)
                .AsQueryable();

            if (categoryId.HasValue)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));

            var products = await query.ToListAsync();

            var result = products.Select(MapToDto).ToList();
            return Ok(result);
        }

        // GET /api/products/5  -> full detail with all vendor price comparisons
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _db.Products
                .Include(p => p.Category)
                .Include(p => p.VendorProducts)
                    .ThenInclude(vp => vp.Vendor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            return Ok(MapToDto(product));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var categoryExists = await _db.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            if (!categoryExists) return BadRequest(new { message = "Invalid category." });

            var product = new Product
            {
                Name = dto.Name,
                CategoryId = dto.CategoryId,
                Unit = dto.Unit,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                BrandName = dto.BrandName
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return Ok(new { product.Id, product.Name });
        }

        // Vendor lists their price/stock for a product -> this powers price comparison
        [HttpPost("vendor-offer")]
        public async Task<IActionResult> AddVendorOffer(AddVendorProductDto dto)
        {
            var productExists = await _db.Products.AnyAsync(p => p.Id == dto.ProductId);
            var vendorExists = await _db.Vendors.AnyAsync(v => v.Id == dto.VendorId);

            if (!productExists) return BadRequest(new { message = "Invalid product." });
            if (!vendorExists) return BadRequest(new { message = "Invalid vendor." });

            var vendorProduct = new VendorProduct
            {
                ProductId = dto.ProductId,
                VendorId = dto.VendorId,
                Price = dto.Price,
                StockQty = dto.StockQty,
                MinOrderQty = dto.MinOrderQty,
                BulkPriceTiersJson = dto.BulkPriceTiersJson,
                IsAvailable = dto.StockQty > 0
            };

            _db.VendorProducts.Add(vendorProduct);
            await _db.SaveChangesAsync();

            return Ok(new { vendorProduct.Id, message = "Vendor offer added." });
        }

        // Helper mapping method
        private static ProductDto MapToDto(Product p)
        {
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                CategoryName = p.Category.Name,
                Unit = p.Unit,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                BrandName = p.BrandName,
                VendorOffers = p.VendorProducts
                    .OrderBy(vp => vp.Price) // cheapest first -> price comparison built-in
                    .Select(vp => new VendorOfferDto
                    {
                        VendorProductId = vp.Id,
                        VendorId = vp.VendorId,
                        VendorShopName = vp.Vendor.ShopName,
                        Price = vp.Price,
                        StockQty = vp.StockQty,
                        MinOrderQty = vp.MinOrderQty,
                        IsAvailable = vp.IsAvailable,
                        VendorRating = vp.Vendor.Rating
                    }).ToList()
            };
        }
    }
}