using MassTransit;
using Microsoft.IdentityModel.Tokens;
using OrderService.Application.Services.Contract;
using OrderStateMachine.Messages.Events;

namespace OrderService.API.Consumer
{
    public class OrderAcceptedConsumer : IConsumer<IOrderAccepted>
    {
        private readonly IOrderAppService _orderService;
        public OrderAcceptedConsumer(IOrderAppService orderService)
        {
            _orderService = orderService;
        }

        public async Task Consume(ConsumeContext<IOrderAccepted> context)
        {
            var order = context.Message;
            await _orderService.AcceptedOrderAsync(order.OrderId, order.AcceptedDateTime);
            
        }
    }
}
