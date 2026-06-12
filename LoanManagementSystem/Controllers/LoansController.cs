using LoanManagementSystem.DTOs;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly LoanService _loanService;

        public LoansController(LoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost("CreateApplication")]
        public async Task<IActionResult> CreateApplication([FromBody] CreateLoanRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _loanService.CreateLoanApplicationAsync(request);
                return CreatedAtAction(nameof(GetLoan), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLoan(int id)
        {
            try
            {
                var result = await _loanService.GetLoanByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost("check-overdue")]
        public async Task<IActionResult> CheckOverdue()
        {
            await _loanService.CheckOverdueLoansAsync();
            return Ok(new { message = "Overdue loans updated." });
        }
    }
}