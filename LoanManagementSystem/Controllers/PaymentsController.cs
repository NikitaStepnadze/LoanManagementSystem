using LoanManagementSystem.DTOs;
using LoanManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly LoanService _loanService;

        public PaymentsController(LoanService loanService)
        {
            _loanService = loanService;
        }
        
        [HttpPost]
        public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _loanService.MakePaymentAsync(request);
                return Ok(new { message = "Payment recorded successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}