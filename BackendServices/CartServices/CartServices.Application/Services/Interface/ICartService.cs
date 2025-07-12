using CartServices.Application.DTOs;
using CartServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServices.Application.Services.Interface
{
    public interface ICartService
    {
        Task<CartDTO> GetUserCartAsync(int userId);
        Task<int> GetCartItemCount(int userId);
        Task<IEnumerable<CartItemDTO>> GetCartItemsAsync(int cartId);
        Task<CartDTO> GetCartAsync(int cartId);
        Task<CartDTO> AddToCartAsync(CartItemDTO cartItem, int userId);
        Task<int> DeleteItemAsync(int itemId, int cartId);
        Task<bool> MakeInActiveAsync(int cartId);
        Task<int> UpdateCartItem(int cartId, int itemId, int Quantity);
    }
}
