using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModitBackend.Data;
using ModitBackend.DTOs;
using ModitBackend.Models;

namespace ModitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendorsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VendorsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Register as a vendor (user must already exist with role Supplier)
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVendorDto dto)
        {
            var user = await _db.Users.FindAsync(dto.UserId);
            if (user == null) return BadRequest(new { message = "User not found." });
            if (user.Role != UserRole.Supplier)
                return BadRequest(new { message = "Only users with Supplier role can register as vendors." });

            var alreadyVendor = await _db.Vendors.AnyAsync(v => v.UserId == dto.UserId);
            if (alreadyVendor) return BadRequest(new { message = "Vendor profile already exists for this user." });

            var vendor = new Vendor
            {
                UserId = dto.UserId,
                ShopName = dto.ShopName,
                ServiceZones = dto.ServiceZones,
                DeliveryRadiusKm = dto.DeliveryRadiusKm,
                Rating = 0,
                IsApproved = false // admin must approve before vendor can sell
            };

            _db.Vendors.Add(vendor);
            await _db.SaveChangesAsync();

            return Ok(new VendorDto
            {
                Id = vendor.Id,
                UserId = vendor.UserId,
                ShopName = vendor.ShopName,
                ServiceZones = vendor.ServiceZones,
                Rating = vendor.Rating,
                DeliveryRadiusKm = vendor.DeliveryRadiusKm,
                IsApproved = vendor.IsApproved
            });
        }

        // Get all vendors (optionally filter by zone/area for Delhi NCR mapping)
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? zone, [FromQuery] bool? approvedOnly)
        {
            var query = _db.Vendors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(zone))
                query = query.Where(v => v.ServiceZones.Contains(zone));

            if (approvedOnly == true)
                query = query.Where(v => v.IsApproved);

            var vendors = await query
                .Select(v => new VendorDto
                {
                    Id = v.Id,
                    UserId = v.UserId,
                    ShopName = v.ShopName,
                    ServiceZones = v.ServiceZones,
                    Rating = v.Rating,
                    DeliveryRadiusKm = v.DeliveryRadiusKm,
                    IsApproved = v.IsApproved
                })
                .ToListAsync();

            return Ok(vendors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vendor = await _db.Vendors.FindAsync(id);
            if (vendor == null) return NotFound();

            return Ok(new VendorDto
            {
                Id = vendor.Id,
                UserId = vendor.UserId,
                ShopName = vendor.ShopName,
                ServiceZones = vendor.ServiceZones,
                Rating = vendor.Rating,
                DeliveryRadiusKm = vendor.DeliveryRadiusKm,
                IsApproved = vendor.IsApproved
            });
        }

        // Admin approves/rejects vendor onboarding
        [HttpPost("approve")]
        public async Task<IActionResult> Approve(ApproveVendorDto dto)
        {
            var vendor = await _db.Vendors.FindAsync(dto.VendorId);
            if (vendor == null) return NotFound();

            vendor.IsApproved = dto.Approve;
            await _db.SaveChangesAsync();

            return Ok(new { vendor.Id, vendor.IsApproved });
        }
    }
}