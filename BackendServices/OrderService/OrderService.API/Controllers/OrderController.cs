using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.API.Messages;
using OrderService.Application.DTOs;
using OrderService.Application.Services.Contract;
using OrderStateMachine.Messages.Commands;

namespace OrderService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderAppService _appService;
        private readonly IConfiguration _config;
        private readonly ISendEndpointProvider _sendEndPointProvider;
        public OrderController(IOrderAppService appService, IConfiguration config, ISendEndpointProvider sendEndpointProvider)
        {
            _appService = appService;
            _config = config;
            _sendEndPointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync(OrderMessage model)
        {
            OrderDTO order= new OrderDTO
            {
                OrderId = Guid.NewGuid(), // Generate a new OrderId
                PaymentId = model.PaymentId,
                UserId = model.UserId,
                CreatedDate = DateTime.UtcNow
            };

            await _appService.SaveOrderAsync(order, model.CartId);
            //Publish order created message to message broker
            // Assuming you have a message broker service to publish messages
            var uri = new Uri($"queue:{_config["ServiceBus:QueueStart"]}");
            var endPoint = await _sendEndPointProvider.GetSendEndpoint(uri);
            await endPoint.Send<IOrderStart>(new
            {
                OrderId = order.OrderId,
                PaymentId = order.PaymentId,
                UserId = order.UserId,
                CartId = model.CartId,
                Products = model.Products
            });
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _appService.GetAllOrdersAsync();
            return Ok(orders);
        }
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var order = await _appService.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetOrdersByUser(long userId)
        {
            var orders = await _appService.GetOrdersByUserAsync(userId);
            return Ok(orders);
        }
        [HttpPost]
        public async Task<IActionResult> SaveOrder(OrderDTO order, long cartId)
        {
            if (order == null)
            {
                return BadRequest("Order cannot be null");
            }
            await _appService.SaveOrderAsync(order, cartId);
            return CreatedAtAction(nameof(GetOrderById), new { orderId = order.OrderId }, order);
        }
        [HttpPost("{orderId}")]
        public async Task<IActionResult> AcceptOrder(Guid orderId, DateTime acceptDateTime)
        {
            var result = await _appService.AcceptedOrderAsync(orderId, acceptDateTime);
            if (!result)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(Guid orderId)
        {
            var result = await _appService.DeleteOrderAsync(orderId);
            if (!result)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
    }
}
