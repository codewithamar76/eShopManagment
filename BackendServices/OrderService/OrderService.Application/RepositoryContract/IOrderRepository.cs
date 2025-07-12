using OrderService.Domain.Entities;

namespace OrderService.Application.RepositoryContract
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(Guid orderId);
        Task<List<Order>> GetOrdersByUserAsync(long UserId);
        Task SaveOrderAsync(Order order, long CartId);
        Task<bool> AcceptedOrderAsync(Guid orderId, DateTime acceptDateTime);
        Task<bool> DeleteOrderAsync(Guid orderId);
    }
}
