using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.DTOs
{
    public class CreateLoanRequest
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [Range(500, 50000, ErrorMessage = "Amount must be between 500 and 50,000")]
        public decimal Amount { get; set; }

        [Required]
        [Range(6, 60, ErrorMessage = "Term must be between 6 and 60 months")]
        public int TermMonths { get; set; }

        [Required]
        [Range(0.1, 100)]
        public decimal InterestRate { get; set; }
    }
}