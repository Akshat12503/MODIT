namespace ModitBackend.Models
{
    public enum VerificationStatus { Pending, Verified, Rejected }

    public class BusinessProfile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string CompanyName { get; set; } = string.Empty;
        public string BusinessType { get; set; } = string.Empty; // Contractor, Architect, etc.
        public string? GstCertificateUrl { get; set; }
        public VerificationStatus VerificationStatus { get; set; } = VerificationStatus.Pending;
    }
}