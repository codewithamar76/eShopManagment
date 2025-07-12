using AutoMapper;
using CartServices.Application.DTOs;
using CartServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServices.Application.Mapper
{
    public class CartMapper:Profile
    {
        public CartMapper() { 
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<CartItem, CartItemDTO>().ReverseMap();
        }
    }
}
