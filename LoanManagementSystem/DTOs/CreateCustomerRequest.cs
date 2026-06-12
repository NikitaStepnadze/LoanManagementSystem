using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.DTOs
{
    public class CreateCustomerRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string PersonalNumber { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        [Range(0, 850)]
        public int CreditScore { get; set; }
    }
}


