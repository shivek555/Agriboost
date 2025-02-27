using System.Threading.Tasks;
using AgriBoostAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgriBoostAPI.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("generate-qr")]
        public async Task<IActionResult> GenerateQRCode([FromBody] PaymentRequest request)
        {
            var payment = await _paymentService.GenerateQRCodeForOrder(request.OrderId, request.Amount);
            return Ok(payment);
        }

        [HttpPost("confirm/{paymentId}")]
        public async Task<IActionResult> ConfirmPayment(int paymentId)
        {
            bool success = await _paymentService.ConfirmPayment(paymentId);
            if (!success) return NotFound("Payment not found.");
            return Ok(new { message = "Payment successful!" });
        }
    }

    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
