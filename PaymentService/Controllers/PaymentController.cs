using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.DTOs;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult ProcessPayment(PaymentDto dto)
        {
            // 🧠 Mock logic
            if (dto.Amount <= 0)
                return BadRequest("Invalid amount");

            // Simulate success/failure
            var isSuccess = dto.Amount < 5000; // example rule

            if (!isSuccess)
                return BadRequest("Payment Failed");

            return Ok(new
            {
                message = "Payment Successful",
                orderId = dto.OrderId
            });
        }
    }
}
