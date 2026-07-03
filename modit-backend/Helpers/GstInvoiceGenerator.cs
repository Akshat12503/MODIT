namespace ModitBackend.Helpers
{
    public static class GstInvoiceGenerator
    {
        private const decimal GstRate = 0.18m; // 18% GST for building materials (standard slab, simplified for prototype)

        public static decimal CalculateGst(decimal amount) => Math.Round(amount * GstRate, 2);

        public static string GenerateInvoiceNumber(int orderId) =>
            $"MODIT-INV-{DateTime.UtcNow:yyyyMMdd}-{orderId:D5}";
    }
}