using LoanManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<LoanSchedule> LoanSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.PersonalNumber)
                .IsUnique();

            modelBuilder.Entity<Loan>()
                .Property(l => l.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Loan>()
                .Property(l => l.InterestRate)
                .HasColumnType("decimal(5,2)");

            modelBuilder.Entity<Loan>()
                .Property(l => l.MonthlyPayment)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<LoanSchedule>()
                .Property(ls => ls.PMT)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Loan>()
                .Property(l => l.Status)
                .HasConversion<string>();
        }
    }
}