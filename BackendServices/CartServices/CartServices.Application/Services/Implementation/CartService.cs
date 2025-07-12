using AutoMapper;
using CartServices.Application.DTOs;
using CartServices.Application.HttpClients;
using CartServices.Application.RepositoryInterface;
using CartServices.Application.Services.Interface;
using CartServices.Domain.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServices.Application.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepo;
        private readonly IMapper _mapper;
        private readonly CatalogServiceClient _serviceClient;
        private readonly IConfiguration _config;
        public CartService(ICartRepository cartRepo, IMapper mapper, CatalogServiceClient serviceClient,
            IConfiguration configuration)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
            _serviceClient = serviceClient;
            _config = configuration;
        }
        public async Task<CartDTO> AddToCartAsync(CartItemDTO cartItem, int userId)
        {
            CartItem item = _mapper.Map<CartItem>(cartItem);
            Cart cart = await _cartRepo.AddToCartAsync(item, userId, item.CartId);
            if (cart == null)
            {
                return null;
            }
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<int> DeleteItemAsync(int itemId, int cartId)
        {
            return await _cartRepo.DeleteItemAsync(itemId, cartId);
        }

        public async Task<CartDTO> GetCartAsync(int cartId)
        {
            Cart cart = await _cartRepo.GetCartAsync(cartId);
            if (cart == null)
            {
                return null;
            }
            CartDTO dTO = PopulateCartDetails(cart);
            return dTO;
        }

        public async Task<int> GetCartItemCount(int userId)
        {
            return await _cartRepo.GetCartItemCount(userId);
        }

        public async Task<IEnumerable<CartItemDTO>> GetCartItemsAsync(int cartId)
        {
            var cartItems = await _cartRepo.GetCartItemsAsync(cartId);
            if (cartItems == null || !cartItems.Any())
            {
                return new List<CartItemDTO>();
            }
            return _mapper.Map<IEnumerable<CartItemDTO>>(cartItems);
        }

        public async Task<CartDTO> GetUserCartAsync(int userId)
        {
            Cart cart = await _cartRepo.GetUserCartAsync(userId);
            CartDTO cartDTO = PopulateCartDetails(cart);
            return cartDTO;
        }

        public async Task<bool> MakeInActiveAsync(int cartId)
        {
            return await _cartRepo.MakeInActiveAsync(cartId);
        }

        public async Task<int> UpdateCartItem(int cartId, int itemId, int Quantity)
        {
            return await _cartRepo.UpdateCartItem(cartId, itemId, Quantity);
        }

        private CartDTO PopulateCartDetails(Cart cart)
        {
            try
            {
                CartDTO cartDTO = _mapper.Map<CartDTO>(cart);
                var ProductIds = cart.CartItems.Select(x => x.ItemId).ToArray();
                var products = _serviceClient.GetByIdsAsync(ProductIds).Result;

                if (cartDTO.CartItems.Count > 0)
                {
                    cartDTO.CartItems.ForEach(x =>
                    {
                        var prod = products.FirstOrDefault(p => p.ProductId == x.ItemId);
                        if (prod != null)
                        {
                            x.Name = prod.Name;
                            x.ImageUrl = prod.ImageUrl;
                        }
                    });

                    foreach (var item in cartDTO.CartItems)
                    {
                        cartDTO.Total += item.UnitPrice * item.Quantity;
                    }
                    cartDTO.Tax = cartDTO.Total * Convert.ToDecimal(_config["Tax"]) / 100;
                    cartDTO.GrandTotal = cartDTO.Total + cartDTO.Tax;
                }
                return cartDTO;
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
    }
}
