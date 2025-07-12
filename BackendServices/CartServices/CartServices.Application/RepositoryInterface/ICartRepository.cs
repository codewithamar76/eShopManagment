using CartServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServices.Application.RepositoryInterface
{
    public interface ICartRepository
    {
        Task<Cart> GetUserCartAsync(int userId);
        Task<int> GetCartItemCount(int userId);
        Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId);
        Task<Cart> GetCartAsync(int cartId);
        Task<Cart> AddToCartAsync(CartItem cartItem, int userId, long cartId);
        Task<int> DeleteItemAsync(int itemId, int cartId);
        Task<bool> MakeInActiveAsync(int cartId);
        Task<int> UpdateCartItem(int cartId, int itemId, int Quantity);
    }
}
