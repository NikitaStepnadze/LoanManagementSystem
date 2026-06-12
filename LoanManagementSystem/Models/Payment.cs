namespace LoanManagementSystem.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public Loan Loan { get; set; } = null!;
    }
}