using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(11)]
        public string PersonalNumber { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public int CreditScore { get; set; }
        public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    }
}