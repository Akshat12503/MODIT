using Microsoft.EntityFrameworkCore;
using ModitBackend.Models;

namespace ModitBackend.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            if (await db.Users.AnyAsync()) return; // already seeded, skip

            // ---------- Categories ----------
            var cement = new Category { Name = "Cement" };
            var steel = new Category { Name = "Steel & TMT Bars" };
            var sandAgg = new Category { Name = "Sand & Aggregate" };
            var bricks = new Category { Name = "Bricks & Blocks" };
            var tiles = new Category { Name = "Tiles" };
            var sanitary = new Category { Name = "Sanitary" };
            var plumbing = new Category { Name = "Plumbing" };
            var electrical = new Category { Name = "Electrical" };
            var paint = new Category { Name = "Paint" };
            var hardware = new Category { Name = "Hardware" };
            var plywood = new Category { Name = "Plywood & Laminates" };
            var glass = new Category { Name = "Glass" };
            var tools = new Category { Name = "Tools" };
            var finishing = new Category { Name = "Finishing Materials" };

            db.Categories.AddRange(cement, steel, sandAgg, bricks, tiles, sanitary,
                plumbing, electrical, paint, hardware, plywood, glass, tools, finishing);
            await db.SaveChangesAsync();

            // ---------- Users (Customers, Contractors, Architect, Suppliers, Admin) ----------
            string Hash(string p) => BCrypt.Net.BCrypt.HashPassword(p);

            var customer1 = new User { Name = "Rohit Sharma", Email = "rohit@example.com", PasswordHash = Hash("Test@123"), Phone = "9810000001", Role = UserRole.Customer, IsVerified = true };
            var contractor1 = new User { Name = "Amit Verma", Email = "amit.contractor@example.com", PasswordHash = Hash("Test@123"), Phone = "9810000002", Role = UserRole.Contractor, CreditLimit = 500000, IsVerified = true };
            var architect1 = new User { Name = "Priya Nair", Email = "priya.architect@example.com", PasswordHash = Hash("Test@123"), Phone = "9810000003", Role = UserRole.Architect, CreditLimit = 200000, IsVerified = true };
            var supplier1User = new User { Name = "Rajesh Traders", Email = "rajesh.supplier@example.com", PasswordHash = Hash("Test@123"), Phone = "9810000004", Role = UserRole.Supplier, IsVerified = true };
            var supplier2User = new User { Name = "NCR BuildMart", Email = "ncr.supplier@example.com", PasswordHash = Hash("Test@123"), Phone = "9810000005", Role = UserRole.Supplier, IsVerified = true };
            var supplier3User = new User { Name = "Gupta Hardware Co", Email = "gupta.supplier@example.com", PasswordHash = Hash("Test@123"), Phone = "9810000006", Role = UserRole.Supplier, IsVerified = true };
            var admin1 = new User { Name = "MODIT Admin", Email = "admin@modit.in", PasswordHash = Hash("Admin@123"), Phone = "9810000000", Role = UserRole.Admin, IsVerified = true };

            db.Users.AddRange(customer1, contractor1, architect1, supplier1User, supplier2User, supplier3User, admin1);
            await db.SaveChangesAsync();

            // Carts for buyers
            db.Carts.AddRange(
                new Cart { UserId = customer1.Id },
                new Cart { UserId = contractor1.Id },
                new Cart { UserId = architect1.Id }
            );

            // Business profiles
            db.BusinessProfiles.AddRange(
                new BusinessProfile { UserId = contractor1.Id, CompanyName = "Verma Constructions", BusinessType = "Contractor", VerificationStatus = VerificationStatus.Verified },
                new BusinessProfile { UserId = architect1.Id, CompanyName = "Nair Design Studio", BusinessType = "Architect", VerificationStatus = VerificationStatus.Verified },
                new BusinessProfile { UserId = supplier1User.Id, CompanyName = "Rajesh Traders Pvt Ltd", BusinessType = "Supplier", VerificationStatus = VerificationStatus.Verified },
                new BusinessProfile { UserId = supplier2User.Id, CompanyName = "NCR BuildMart Pvt Ltd", BusinessType = "Supplier", VerificationStatus = VerificationStatus.Verified },
                new BusinessProfile { UserId = supplier3User.Id, CompanyName = "Gupta Hardware Co", BusinessType = "Supplier", VerificationStatus = VerificationStatus.Verified }
            );
            await db.SaveChangesAsync();

            // ---------- Service Zones (Delhi NCR) ----------
            var zones = new[]
            {
                new ServiceZone { PincodeOrArea = "South Delhi", City = "Delhi NCR", ZoneName = "South Delhi" },
                new ServiceZone { PincodeOrArea = "Gurugram", City = "Delhi NCR", ZoneName = "Gurugram" },
                new ServiceZone { PincodeOrArea = "Noida", City = "Delhi NCR", ZoneName = "Noida" },
                new ServiceZone { PincodeOrArea = "Ghaziabad", City = "Delhi NCR", ZoneName = "Ghaziabad" },
                new ServiceZone { PincodeOrArea = "Faridabad", City = "Delhi NCR", ZoneName = "Faridabad" }
            };
            db.ServiceZones.AddRange(zones);
            await db.SaveChangesAsync();

            // ---------- Vendors ----------
            var vendor1 = new Vendor { UserId = supplier1User.Id, ShopName = "Rajesh Traders", ServiceZones = "South Delhi,Gurugram", Rating = 4.5, DeliveryRadiusKm = 15, IsApproved = true };
            var vendor2 = new Vendor { UserId = supplier2User.Id, ShopName = "NCR BuildMart", ServiceZones = "Noida,Ghaziabad", Rating = 4.2, DeliveryRadiusKm = 20, IsApproved = true };
            var vendor3 = new Vendor { UserId = supplier3User.Id, ShopName = "Gupta Hardware Co", ServiceZones = "Faridabad,South Delhi", Rating = 4.7, DeliveryRadiusKm = 10, IsApproved = true };

            db.Vendors.AddRange(vendor1, vendor2, vendor3);
            await db.SaveChangesAsync();

            // ---------- Products ----------
            var p1 = new Product { Name = "UltraTech Cement (OPC 53 Grade)", CategoryId = cement.Id, Unit = "bag", BrandName = "UltraTech", Description = "50kg bag, high strength cement" };
            var p2 = new Product { Name = "ACC Cement (PPC)", CategoryId = cement.Id, Unit = "bag", BrandName = "ACC", Description = "50kg bag, Portland Pozzolana Cement" };
            var p3 = new Product { Name = "TMT Steel Bar 12mm", CategoryId = steel.Id, Unit = "kg", BrandName = "Tata Tiscon", Description = "Fe 500D grade TMT bar" };
            var p4 = new Product { Name = "River Sand", CategoryId = sandAgg.Id, Unit = "cubic ft", BrandName = "Local", Description = "Fine river sand for construction" };
            var p5 = new Product { Name = "Red Clay Bricks", CategoryId = bricks.Id, Unit = "piece", BrandName = "Local", Description = "Standard size 9x4x3 inch" };
            var p6 = new Product { Name = "Vitrified Floor Tiles 2x2 ft", CategoryId = tiles.Id, Unit = "sqft", BrandName = "Kajaria", Description = "Glossy finish floor tiles" };
            var p7 = new Product { Name = "Asian Paints Tractor Emulsion", CategoryId = paint.Id, Unit = "litre", BrandName = "Asian Paints", Description = "Interior wall emulsion paint" };
            var p8 = new Product { Name = "PVC Pipe 1 inch", CategoryId = plumbing.Id, Unit = "piece", BrandName = "Finolex", Description = "10ft length PVC pipe" };
            var p9 = new Product { Name = "Copper Electrical Wire 1.5mm", CategoryId = electrical.Id, Unit = "coil", BrandName = "Havells", Description = "90m coil, ISI marked" };
            var p10 = new Product { Name = "Plywood Board 19mm", CategoryId = plywood.Id, Unit = "sheet", BrandName = "CenturyPly", Description = "8x4 ft waterproof plywood" };

            db.Products.AddRange(p1, p2, p3, p4, p5, p6, p7, p8, p9, p10);
            await db.SaveChangesAsync();

            // ---------- Vendor Product Offers (multi-vendor pricing for comparison) ----------
            var vendorProducts = new[]
            {
                new VendorProduct { ProductId = p1.Id, VendorId = vendor1.Id, Price = 380, StockQty = 500, MinOrderQty = 10, IsAvailable = true },
                new VendorProduct { ProductId = p1.Id, VendorId = vendor2.Id, Price = 375, StockQty = 300, MinOrderQty = 10, IsAvailable = true },
                new VendorProduct { ProductId = p1.Id, VendorId = vendor3.Id, Price = 390, StockQty = 200, MinOrderQty = 5, IsAvailable = true },

                new VendorProduct { ProductId = p2.Id, VendorId = vendor1.Id, Price = 350, StockQty = 400, MinOrderQty = 10, IsAvailable = true },
                new VendorProduct { ProductId = p2.Id, VendorId = vendor2.Id, Price = 345, StockQty = 250, MinOrderQty = 10, IsAvailable = true },

                new VendorProduct { ProductId = p3.Id, VendorId = vendor1.Id, Price = 68, StockQty = 5000, MinOrderQty = 100, IsAvailable = true },
                new VendorProduct { ProductId = p3.Id, VendorId = vendor3.Id, Price = 66, StockQty = 3000, MinOrderQty = 100, IsAvailable = true },

                new VendorProduct { ProductId = p4.Id, VendorId = vendor2.Id, Price = 45, StockQty = 10000, MinOrderQty = 50, IsAvailable = true },
                new VendorProduct { ProductId = p5.Id, VendorId = vendor1.Id, Price = 8, StockQty = 20000, MinOrderQty = 500, IsAvailable = true },
                new VendorProduct { ProductId = p5.Id, VendorId = vendor3.Id, Price = 7.5m, StockQty = 15000, MinOrderQty = 500, IsAvailable = true },

                new VendorProduct { ProductId = p6.Id, VendorId = vendor2.Id, Price = 55, StockQty = 3000, MinOrderQty = 50, IsAvailable = true },
                new VendorProduct { ProductId = p7.Id, VendorId = vendor1.Id, Price = 210, StockQty = 800, MinOrderQty = 5, IsAvailable = true },
                new VendorProduct { ProductId = p8.Id, VendorId = vendor3.Id, Price = 120, StockQty = 1200, MinOrderQty = 10, IsAvailable = true },
                new VendorProduct { ProductId = p9.Id, VendorId = vendor2.Id, Price = 1450, StockQty = 400, MinOrderQty = 2, IsAvailable = true },
                new VendorProduct { ProductId = p10.Id, VendorId = vendor1.Id, Price = 2200, StockQty = 150, MinOrderQty = 1, IsAvailable = true },
            };

            db.VendorProducts.AddRange(vendorProducts);
            await db.SaveChangesAsync();
        }
    }
}