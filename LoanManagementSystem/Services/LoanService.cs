using LoanManagementSystem.Data;
using LoanManagementSystem.DTOs;
using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Services
{
    public class LoanService
    {
        private readonly AppDbContext _context;

        public LoanService(AppDbContext context)
        {
            _context = context;
        }

        public decimal CalculateMonthlyPayment(decimal principal, decimal annualRatePercent, int months)
        {
            if (annualRatePercent == 0)
                return principal / months;

            double r = (double)(annualRatePercent / 100) / 12;
            double n = months;
            double pmt = (double)principal * (r * Math.Pow(1 + r, n)) / (Math.Pow(1 + r, n) - 1);
            return Math.Round((decimal)pmt, 2);
        }

        public async Task<LoanResponse> CreateLoanApplicationAsync(CreateLoanRequest request)
        {
            var customer = await _context.Customers.FindAsync(request.CustomerId);
            if (customer == null)
                throw new Exception("Customer not found.");

            var age = DateTime.Today.Year - customer.BirthDate.Year;
            if (customer.BirthDate.Date > DateTime.Today.AddYears(-age)) age--;
            if (age < 18)
                throw new Exception("Customer must be at least 18 years old.");

            var status = LoanStatus.Pending;

            if (age < 18)
                throw new Exception("Customer must be at least 18 years old.");

            status = customer.CreditScore < 300 ? LoanStatus.Rejected : LoanStatus.Approved;

            var monthlyPayment = CalculateMonthlyPayment(request.Amount, request.InterestRate, request.TermMonths);

            var loan = new Loan
            {
                CustomerId = request.CustomerId,
                Amount = request.Amount,
                InterestRate = request.InterestRate,
                TermMonths = request.TermMonths,
                MonthlyPayment = monthlyPayment,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            _context.Loans.Add(loan);
            await _context.SaveChangesAsync(); 

            if (status == LoanStatus.Approved)
            {
                var scheduleEntries = new List<LoanSchedule>();
                for (int i = 1; i <= request.TermMonths; i++)
                {
                    scheduleEntries.Add(new LoanSchedule
                    {
                        LoanId = loan.Id,
                        PMT = monthlyPayment,
                        DueDate = DateTime.UtcNow.AddMonths(i),
                        IsPaid = false
                    });
                }
                _context.LoanSchedules.AddRange(scheduleEntries);
                await _context.SaveChangesAsync();
            }

            return new LoanResponse
            {
                Id = loan.Id,
                CustomerId = loan.CustomerId,
                CustomerName = $"{customer.FirstName} {customer.LastName}",
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                TermMonths = loan.TermMonths,
                MonthlyPayment = loan.MonthlyPayment,
                Status = loan.Status.ToString(),
                CreatedAt = loan.CreatedAt
            };
        }

        public async Task<bool> MakePaymentAsync(MakePaymentRequest request)
        {
            var loan = await _context.Loans.FindAsync(request.LoanId);
            if (loan == null)
                throw new Exception("Loan not found.");

            if (loan.Status == LoanStatus.Closed)
                throw new Exception("Cannot make a payment on a closed loan.");

            var payment = new Payment
            {
                LoanId = request.LoanId,
                Amount = request.Amount,
                PaymentDate = DateTime.UtcNow
            };
            _context.Payments.Add(payment);

            var totalPaid = await _context.Payments
                .Where(p => p.LoanId == request.LoanId)
                .SumAsync(p => p.Amount);

            totalPaid += request.Amount; 

            if (totalPaid >= loan.Amount)
                loan.Status = LoanStatus.Closed;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LoanResponse> GetLoanByIdAsync(int id)
        {
            var loan = await _context.Loans
                .Include(l => l.Customer)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (loan == null)
                throw new Exception("Loan not found.");

            return new LoanResponse
            {
                Id = loan.Id,
                CustomerId = loan.CustomerId,
                CustomerName = $"{loan.Customer.FirstName} {loan.Customer.LastName}",
                Amount = loan.Amount,
                InterestRate = loan.InterestRate,
                TermMonths = loan.TermMonths,
                MonthlyPayment = loan.MonthlyPayment,
                Status = loan.Status.ToString(),
                CreatedAt = loan.CreatedAt
            };
        }

        public async Task CheckOverdueLoansAsync()
        {
            var activeLoans = await _context.Loans
                .Where(l => l.Status == LoanStatus.Approved)
                .Include(l => l.Schedule)
                .ToListAsync();

            foreach (var loan in activeLoans)
            {
                var hasOverdue = loan.Schedule
                    .Any(s => s.DueDate < DateTime.UtcNow && !s.IsPaid);

                if (hasOverdue)
                    loan.Status = LoanStatus.Overdue;
            }

            await _context.SaveChangesAsync();
        }
    }
}