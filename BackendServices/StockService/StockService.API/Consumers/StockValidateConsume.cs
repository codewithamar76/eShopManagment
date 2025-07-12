using MassTransit;
using OrderStateMachine.Messages.Commands;
using OrderStateMachine.Messages.Events;
using StockService.Application.Services.Contract;

namespace StockService.API.Consumers
{
    public class StockValidateConsume : IConsumer<IStockValidate>
    {
        private readonly IStockAppService _stockService;
        public StockValidateConsume(IStockAppService stockService)
        {
            _stockService = stockService;
        }
        public async Task Consume(ConsumeContext<IStockValidate> context)
        {
            try
            {
                var message = context.Message;
                if (message != null)
                {
                    var products = message.Products.Split(",").ToArray();
                    var isStockAvailable = true;
                    foreach (var product in products)
                    {
                        var productDetails = product.Split(":");
                        if (productDetails.Length == 2)
                        {
                            var productId = int.Parse(productDetails[0]);
                            var quantity = int.Parse(productDetails[1]);
                            isStockAvailable = await _stockService.CheckStockAvailablity(productId, quantity);
                            if (!isStockAvailable)
                            {
                                break;
                            }
                        }
                    }

                    if (!isStockAvailable)
                    {
                        //Cancel order
                        await context.Publish<IOrderCanceled>(new
                        {
                            OrderId = message.OrderId,
                            PaymentId = message.PaymentId,
                            CartID = message.CartId,
                        });
                    }
                    else
                    {
                        //Reserve stock
                        foreach (var product in products)
                        {
                            var productDetails = product.Split(":");
                            if (productDetails.Length == 2)
                            {
                                var productId = int.Parse(productDetails[0]);
                                var quantity = int.Parse(productDetails[1]);
                                await _stockService.ReserveStock(productId, quantity);
                            }
                        }
                        //Publish order started event
                        await context.Publish<IOrderAccepted>(new
                        {
                            OrderId = message.OrderId,
                            PaymentId = message.PaymentId,
                            Products = message.Products,
                            CartId = message.CartId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                Console.WriteLine($"Error processing stock validation: {ex.Message}");
                // Optionally, you can publish an event to notify about the failure
                await context.Publish<IOrderCanceled>(new
                {
                    OrderId = context.Message.OrderId,
                    PaymentId = context.Message.PaymentId,
                    CartID = context.Message.CartId,
                });
            }
        }
    }
}
