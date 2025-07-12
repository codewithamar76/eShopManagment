using PaymentService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services.Contract
{
    public interface IPaymentAppService
    {
        Task<bool> SavePaymentDetailsAsync(PaymentDetailsDTO payment);
        string CreateOrder(RazorPayOrderDTO orderDTO);
        //Payment GetPaymentDetails(string paymentId);
        string VerifyPayment(PaymentConfirmDTO payment);
    }
}
