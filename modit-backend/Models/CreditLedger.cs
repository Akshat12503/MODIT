namespace ModitBackend.Models
{
    public enum TransactionType { Credit, Debit }

    public class CreditLedger
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsSettled { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}