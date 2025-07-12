using Microsoft.EntityFrameworkCore;
using OrderService.Application.RepositoryContract;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;
        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AcceptedOrderAsync(Guid orderId, DateTime acceptDateTime)
        {
            try
            {
                var order = await _context.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order != null)
                {
                    order.AcceptDate = acceptDateTime;
                    _context.Update(order);
                    return await _context.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                throw new Exception("An error occurred while accepting the order.", ex);
            }
            return false;
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return await _context.Orders
                .Where(o => o.OrderId == orderId)
                .ExecuteDeleteAsync() > 0;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public async Task<List<Order>> GetOrdersByUserAsync(long UserId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == UserId)
                .ToListAsync();
        }

        public async Task SaveOrderAsync(Order order, long CartId)
        {
            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                throw new Exception("An error occurred while saving the order.", ex);
            }
        }
    }
}
