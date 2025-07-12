using eShopFlix.Helper;
using eShopFlix.HttpClients;
using eShopFlix.Message;
using eShopFlix.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopFlix.Controllers
{
    public class PaymentController : BaseController
    {
        private readonly PaymentServiceClient _payment;
        private readonly CartServiceClient _cart;
        private readonly OrderServiceClient _orderService;
        private readonly IConfiguration _config;
        public PaymentController(PaymentServiceClient payment, CartServiceClient cart, IConfiguration config, OrderServiceClient orderService)
        {
            _payment = payment;
            _cart = cart;
            _config = config;
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            if (CurrentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            CartModel cart = _cart.GetUserCartAsync(CurrentUser.UserId).Result;
            if (cart != null)
            {
                PaymentModel payment = new PaymentModel();
                payment.Cart = cart;
                payment.Name = CurrentUser.Name;
                payment.RazorpayKey = _config["RazorPay:Key"];
                payment.GrandTotal = cart.GrandTotal;
                payment.Currency = "INR";
                payment.Description = string.Join(", ", cart.CartItems.Select(c => c.Name));
                payment.Receipt = Guid.NewGuid().ToString();

                RazorPayOrderModel orderModel = new RazorPayOrderModel
                {
                    Amount = (int)(payment.GrandTotal * 100), // Convert to paise
                    Currency = payment.Currency,
                    Receipt = payment.Receipt
                };
                
                payment.OrderId = _payment.CreateOrderAsync(orderModel).Result;
                return View(payment);
            }
            return RedirectToAction("Index", "Cart");
        }

        public async Task<IActionResult> Status(IFormCollection form)
        {
            if (!string.IsNullOrEmpty(form["rzp_paymentid"]))
            {
                string paymentId = form["rzp_paymentid"];
                string orderId = form["rzp_orderid"];
                string signature = form["rzp_signature"];
                string transactionId = form["Receipt"];
                string currency = form["currency"];

                PaymentConfirmModel confirmModel = new PaymentConfirmModel
                {
                    PaymentId = paymentId,
                    OrderId = orderId,
                    Signature = signature,
                };
                string status = _payment.VerifyPayment(confirmModel).Result;

                if (status == "captured" || status == "completed")
                {
                    CartModel cart = _cart.GetUserCartAsync(CurrentUser.UserId).Result;
                    TransactionModel transaction = new TransactionModel();
                    transaction.UserId = CurrentUser.UserId;
                    transaction.TransactionId = transactionId;
                    transaction.Currency = currency;
                    transaction.Status = status;
                    transaction.GrandTotal = cart.GrandTotal;
                    transaction.CartId = (int)cart.Id;
                    transaction.CreatedDate = DateTime.UtcNow;
                    transaction.Email = CurrentUser.Email;
                    transaction.Total = cart.Total;
                    transaction.Tax = cart.Tax;
                    transaction.Id = paymentId;

                    bool result = _payment.SavePaymentDetailAsync(transaction).Result;
                    if (result)
                    {
                        // Order Workflow
                        OrderMessage orderMessage = new OrderMessage
                        {
                            UserId = CurrentUser.UserId,
                            PaymentId = paymentId,
                            CartId = (int)cart.Id,
                            Products = string.Join(",", cart.CartItems.Select(c => $"{c.ItemId}:{c.Quantity}"))
                        };
                        await _orderService.CreateOrderAsync(orderMessage);
                        await _cart.MakeCartInActiveAsync(Convert.ToInt32(cart.Id));
                        TempData.Set("Receipt", transaction);
                        return RedirectToAction("Receipt");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Payment details could not be saved. Please try again.";
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Payment verification failed. Please try again.";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Payment details are missing. Please try again.";
            }
            return View();
        }

        public IActionResult Receipt()
        {
            TransactionModel transaction = TempData.Get<TransactionModel>("Receipt");
            if (transaction != null)
            {
                return View(transaction);
            }
            else
            {
                ViewBag.ErrorMessage = "No transaction details found.";
                return View();
            }
        }
    }
}
