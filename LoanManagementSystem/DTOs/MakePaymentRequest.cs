using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.DTOs
{
    public class MakePaymentRequest
    {
        [Required]
        public int LoanId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
        public decimal Amount { get; set; }
    }
}