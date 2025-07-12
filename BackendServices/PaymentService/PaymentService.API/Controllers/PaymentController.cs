using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.DTOs;
using PaymentService.Application.Services.Contract;

namespace PaymentService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentAppService paymentAppService;
        public PaymentController(IPaymentAppService paymentAppService)
        {
            this.paymentAppService = paymentAppService;
        }

        [HttpPost]
        public async Task<IActionResult> SavePaymentDetails(PaymentDetailsDTO payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment details cannot be null.");
            }
            bool result = await paymentAppService.SavePaymentDetailsAsync(payment);
            if (result)
            {
                return Ok(result);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving payment details.");
        }

        [HttpPost]
        public IActionResult CreateOrder(RazorPayOrderDTO orderDTO)
        {
            if (orderDTO == null)
            {
                return BadRequest("Order details cannot be null.");
            }
            string orderId = paymentAppService.CreateOrder(orderDTO);
            if (!string.IsNullOrEmpty(orderId))
            {
                return Ok(orderId);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the order.");
        }
        [HttpPost]
        public IActionResult VerifyPayment(PaymentConfirmDTO payment)
        {
            if (payment == null)
            {
                return BadRequest("Payment confirmation details cannot be null.");
            }
            string verificationResult = paymentAppService.VerifyPayment(payment);
            if (!string.IsNullOrEmpty(verificationResult))
            {
                return Ok(verificationResult);
            }
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while verifying the payment.");
        }
    }
}
