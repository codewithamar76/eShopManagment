using AutoMapper;
using OrderService.Application.DTOs;
using OrderService.Application.RepositoryContract;
using OrderService.Application.Services.Contract;
using OrderService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Services.Implementation
{
    public class OrderAppService:IOrderAppService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public OrderAppService(IOrderRepository orderRepo, IMapper mapper)
        {
            _orderRepo = orderRepo;
            _mapper = mapper;
        }

        public async Task<bool> AcceptedOrderAsync(Guid orderId, DateTime acceptDateTime)
        {
            return await _orderRepo.AcceptedOrderAsync(orderId, acceptDateTime);
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return await _orderRepo.DeleteOrderAsync(orderId);
        }

        public async Task<List<OrderDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return _mapper.Map<List<OrderDTO>>(orders);
        }

        public async Task<OrderDTO> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _orderRepo.GetOrderByIdAsync(orderId);
            return _mapper.Map<OrderDTO>(order);
        }

        public async Task<List<OrderDTO>> GetOrdersByUserAsync(long UserId)
        {
            var Orders = await _orderRepo.GetOrdersByUserAsync(UserId);
            return _mapper.Map<List<OrderDTO>>(Orders);
        }

        public async Task SaveOrderAsync(OrderDTO order, long CartId)
        {
            Order order1 = _mapper.Map<Order>(order);
            await _orderRepo.SaveOrderAsync(order1, CartId);
        }
    }
}
