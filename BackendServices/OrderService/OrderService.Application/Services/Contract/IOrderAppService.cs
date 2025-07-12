using OrderService.Application.DTOs;


namespace OrderService.Application.Services.Contract
{
    public interface IOrderAppService
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO> GetOrderByIdAsync(Guid orderId);
        Task<List<OrderDTO>> GetOrdersByUserAsync(long UserId);
        Task SaveOrderAsync(OrderDTO order, long CartId);
        Task<bool> AcceptedOrderAsync(Guid orderId, DateTime acceptDateTime);
        Task<bool> DeleteOrderAsync(Guid orderId);
    }
}
