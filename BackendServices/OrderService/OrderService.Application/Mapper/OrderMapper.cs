using AutoMapper;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mapper
{
    public class OrderMapper:Profile
    {
        public OrderMapper()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
        }
    }
}
