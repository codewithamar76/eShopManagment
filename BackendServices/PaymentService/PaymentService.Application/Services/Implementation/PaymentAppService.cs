using AutoMapper;
using Microsoft.Extensions.Configuration;
using PaymentService.Application.DTOs;
using PaymentService.Application.RepositoryContract;
using PaymentService.Application.Services.Contract;
using PaymentService.Domain.Entities;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Services.Implementation
{
    public class PaymentAppService : IPaymentAppService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly RazorpayClient _razorpay;
        public PaymentAppService(IPaymentRepository paymentRepository
            , IConfiguration configuration, IMapper mapper)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _config = configuration;
            _razorpay = new RazorpayClient(
                _config["RazorPay:Key"],
                _config["RazorPay:Secret"]
            );
        }

        public string CreateOrder(RazorPayOrderDTO orderDTO)
        {
            Dictionary<string, object> paymentData = new Dictionary<string, object>
            {
                { "amount", orderDTO.Amount }, // Convert to paise
                { "currency", orderDTO.Currency },
                { "receipt", orderDTO.Receipt },
            };

            Order data = _razorpay.Order.Create(paymentData);
            return data["id"].ToString();
        }

        public async Task<bool> SavePaymentDetailsAsync(PaymentDetailsDTO payment)
        {
            PaymentDetail detail = _mapper.Map<PaymentDetail>(payment);
            detail.CreatedDate = DateTime.UtcNow;
            return await _paymentRepository.SavePaymentDetails(detail);
        }

        public string VerifyPayment(PaymentConfirmDTO payment)
        {
            string payload = string.Format("{0}|{1}", payment.OrderId, payment.PaymentId);
            string secret = RazorpayClient.Secret;
            string signature = GetSignature(payload, secret);
            bool status = signature.Equals(payment.Signature);

            if (status)
            {
                Payment paymentDetails = GetPaymentDetails(payment.PaymentId);
                return paymentDetails["status"].ToString();
            }
            return "";
        }

        private Payment GetPaymentDetails(string paymentId)
        {
            return _razorpay.Payment.Fetch(paymentId);
        }

        private string GetSignature(string payload, string secret)
        {
            byte[] secByte = StringEncode(secret);
            HMACSHA256 hmac = new HMACSHA256(secByte);
            var bytes = StringEncode(payload);

            return HashEncode(hmac.ComputeHash(bytes));
        }

        private static byte[] StringEncode(string text)
        {
            var encoding = new ASCIIEncoding();
            return encoding.GetBytes(text);
        }
        private static string HashEncode(byte[] hash)
        {
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }
}
