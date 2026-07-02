using Microsoft.EntityFrameworkCore;
using ModitBackend.Data;
using ModitBackend.DTOs;
using ModitBackend.Helpers;
using ModitBackend.Models;

namespace ModitBackend.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly JwtTokenGenerator _jwtGenerator;

        public AuthService(ApplicationDbContext db, JwtTokenGenerator jwtGenerator)
        {
            _db = db;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
                return null; // caller will translate this to "email already exists"

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Phone = request.Phone,
                Role = request.Role,
                GSTNumber = request.GSTNumber,
                IsVerified = request.Role == UserRole.Customer // customers auto-verified, others need admin approval
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Create business profile for Contractor/Architect/Supplier roles
            if (request.Role is UserRole.Contractor or UserRole.Architect or UserRole.Supplier)
            {
                var profile = new BusinessProfile
                {
                    UserId = user.Id,
                    CompanyName = request.CompanyName ?? string.Empty,
                    BusinessType = request.BusinessType ?? request.Role.ToString(),
                    VerificationStatus = VerificationStatus.Pending
                };
                _db.BusinessProfiles.Add(profile);
                await _db.SaveChangesAsync();
            }

            // Also create an empty cart for the new user
            _db.Carts.Add(new Cart { UserId = user.Id });
            await _db.SaveChangesAsync();

            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return null;

            bool validPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!validPassword)
                return null;

            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role.ToString(),
                Token = token
            };
        }
    }
}