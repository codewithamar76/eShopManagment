using MassTransit;
using OrderService.Application.Services.Contract;
using OrderStateMachine.Messages.Events;

namespace OrderService.API.Consumer
{
    public class OrderCancelledConsumer : IConsumer<IOrderCanceled>
    {
        private readonly IOrderAppService _orderService;
        public OrderCancelledConsumer(IOrderAppService orderService)
        {
            _orderService = orderService;
        }
        public async Task Consume(ConsumeContext<IOrderCanceled> context)
        {
            var order = context.Message;
            await _orderService.DeleteOrderAsync(order.OrderId);
        }
    }
}
