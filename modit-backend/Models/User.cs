namespace ModitBackend.Models
{
    public enum UserRole { Customer, Contractor, Architect, Supplier, Admin }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public decimal CreditLimit { get; set; } = 0;
        public string? GSTNumber { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public BusinessProfile? BusinessProfile { get; set; }
        public Vendor? Vendor { get; set; }
    }
}