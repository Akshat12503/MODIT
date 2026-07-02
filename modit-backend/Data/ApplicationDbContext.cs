using Microsoft.EntityFrameworkCore;
using ModitBackend.Models;

namespace ModitBackend.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<BusinessProfile> BusinessProfiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<VendorProduct> VendorProducts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<QuotationRequest> QuotationRequests { get; set; }
        public DbSet<QuotationResponse> QuotationResponses { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<CreditLedger> CreditLedgers { get; set; }
        public DbSet<ServiceZone> ServiceZones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Prevent multiple cascade paths (SQL Server restriction) ----------
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuotationRequest>()
                .HasOne(q => q.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreditLedger>()
                .HasOne(c => c.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.VendorProduct)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Unique constraint ----------
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ---------- Enum-to-string storage (more readable in DB than int codes) ----------
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(o => o.PaymentMode)
                .HasConversion<string>();

            // ---------- Decimal precision (fixes EF Core warnings) ----------
            modelBuilder.Entity<User>()
                .Property(u => u.CreditLimit)
                .HasPrecision(18, 2);

            modelBuilder.Entity<VendorProduct>()
                .Property(vp => vp.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.PriceAtOrder)
                .HasPrecision(18, 2);

            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.QuotedPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.GstAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CreditLedger>()
                .Property(cl => cl.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<CreditLedger>()
                .Property(cl => cl.Balance)
                .HasPrecision(18, 2);
        }
    }
}