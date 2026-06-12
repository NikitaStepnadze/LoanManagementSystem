using LoanManagementSystem.Data;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CustomersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var age = DateTime.Today.Year - request.BirthDate.Year;
            if (request.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;
            if (age < 18)
                return BadRequest(new { message = "Customer must be at least 18 years old." });

            var exists = await _context.Customers
                .AnyAsync(c => c.PersonalNumber == request.PersonalNumber);
            if (exists)
                return Conflict(new { message = "A customer with this personal number already exists." });

            var customer = new Customer
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PersonalNumber = request.PersonalNumber,
                BirthDate = request.BirthDate,
                CreditScore = request.CreditScore
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCustomerLoans), new { customerId = customer.Id }, customer);
        }

        [HttpGet("loans")]
        public async Task<IActionResult> GetCustomerLoans([FromQuery] int customerId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            var loans = await _context.Loans
                .Where(l => l.CustomerId == customerId)
                .Select(l => new LoanResponse
                {
                    Id = l.Id,
                    CustomerId = l.CustomerId,
                    CustomerName = $"{l.Customer.FirstName} {l.Customer.LastName}",
                    Amount = l.Amount,
                    InterestRate = l.InterestRate,
                    TermMonths = l.TermMonths,
                    MonthlyPayment = l.MonthlyPayment,
                    Status = l.Status.ToString(),
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();

            return Ok(loans);
        }
    }
}