using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModitBackend.Data;
using ModitBackend.DTOs;
using ModitBackend.Models;

namespace ModitBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuotationsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public QuotationsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Customer/Contractor posts a requirement
        [HttpPost("request")]
        public async Task<IActionResult> CreateRequest(CreateQuotationRequestDto dto)
        {
            var userExists = await _db.Users.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists) return BadRequest(new { message = "Invalid user." });

            var request = new QuotationRequest
            {
                UserId = dto.UserId,
                ProjectDescription = dto.ProjectDescription,
                RequiredMaterialsJson = JsonSerializer.Serialize(dto.RequiredMaterials),
                Deadline = dto.Deadline,
                Status = QuotationRequestStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            _db.QuotationRequests.Add(request);
            await _db.SaveChangesAsync();

            return Ok(new { request.Id, message = "Quotation request posted." });
        }

        // Vendors browse open requests (could later filter by their service zone/category)
        [HttpGet("open")]
        public async Task<IActionResult> GetOpenRequests()
        {
            var requests = await _db.QuotationRequests
                .Include(r => r.Responses)
                    .ThenInclude(resp => resp.Vendor)
                .Where(r => r.Status == QuotationRequestStatus.Open)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests.Select(MapToDto).ToList());
        }

        // Get all requests posted by a specific user, with all vendor responses -> comparison view
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserRequests(int userId)
        {
            var requests = await _db.QuotationRequests
                .Include(r => r.Responses)
                    .ThenInclude(resp => resp.Vendor)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return Ok(requests.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var request = await _db.QuotationRequests
                .Include(r => r.Responses)
                    .ThenInclude(resp => resp.Vendor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null) return NotFound();
            return Ok(MapToDto(request));
        }

        // Vendor submits a quote against a request
        [HttpPost("respond")]
        public async Task<IActionResult> SubmitResponse(SubmitQuotationResponseDto dto)
        {
            var request = await _db.QuotationRequests.FindAsync(dto.QuotationRequestId);
            if (request == null) return BadRequest(new { message = "Quotation request not found." });
            if (request.Status != QuotationRequestStatus.Open)
                return BadRequest(new { message = "This request is no longer open." });

            var vendor = await _db.Vendors.FindAsync(dto.VendorId);
            if (vendor == null || !vendor.IsApproved)
                return BadRequest(new { message = "Vendor not found or not approved." });

            var response = new QuotationResponse
            {
                QuotationRequestId = dto.QuotationRequestId,
                VendorId = dto.VendorId,
                QuotedPrice = dto.QuotedPrice,
                DeliveryTimeEstimate = dto.DeliveryTimeEstimate,
                Notes = dto.Notes,
                Status = QuotationResponseStatus.Submitted,
                CreatedAt = DateTime.UtcNow
            };

            _db.QuotationResponses.Add(response);
            await _db.SaveChangesAsync();

            return Ok(new { response.Id, message = "Quote submitted." });
        }

        // Customer accepts one vendor's quote -> closes the request, rejects the rest
        [HttpPost("respond-to-quote")]
        public async Task<IActionResult> RespondToQuote(RespondToQuoteDto dto)
        {
            var response = await _db.QuotationResponses
                .Include(r => r.QuotationRequest)
                .FirstOrDefaultAsync(r => r.Id == dto.QuotationResponseId);

            if (response == null) return NotFound();

            if (dto.Accept)
            {
                response.Status = QuotationResponseStatus.Accepted;
                response.QuotationRequest.Status = QuotationRequestStatus.Fulfilled;

                // Auto-reject all other responses to this request
                var otherResponses = await _db.QuotationResponses
                    .Where(r => r.QuotationRequestId == response.QuotationRequestId && r.Id != response.Id)
                    .ToListAsync();

                foreach (var other in otherResponses)
                    other.Status = QuotationResponseStatus.Rejected;
            }
            else
            {
                response.Status = QuotationResponseStatus.Rejected;
            }

            await _db.SaveChangesAsync();
            return Ok(new { response.Id, response.Status });
        }

        private static QuotationRequestDto MapToDto(QuotationRequest r)
        {
            return new QuotationRequestDto
            {
                Id = r.Id,
                ProjectDescription = r.ProjectDescription,
                RequiredMaterials = JsonSerializer.Deserialize<List<RequiredMaterialItem>>(r.RequiredMaterialsJson) ?? new(),
                Deadline = r.Deadline,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                Responses = r.Responses
                    .OrderBy(resp => resp.QuotedPrice) // cheapest quote first -> comparison built-in
                    .Select(resp => new QuotationResponseDto
                    {
                        Id = resp.Id,
                        VendorId = resp.VendorId,
                        VendorShopName = resp.Vendor.ShopName,
                        VendorRating = resp.Vendor.Rating,
                        QuotedPrice = resp.QuotedPrice,
                        DeliveryTimeEstimate = resp.DeliveryTimeEstimate,
                        Notes = resp.Notes,
                        Status = resp.Status.ToString(),
                        CreatedAt = resp.CreatedAt
                    }).ToList()
            };
        }
    }
}