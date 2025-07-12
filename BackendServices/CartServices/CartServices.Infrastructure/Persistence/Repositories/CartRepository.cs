using CartServices.Application.RepositoryInterface;
using CartServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartServices.Infrastructure.Persistence.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CartDbContext _context;
        public CartRepository(CartDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> AddToCartAsync(CartItem cartItem, int userId, long cartId)
        {
            Cart cart = new Cart();
            if (cartId > 0)
            {
                cart = await _context.Carts.FindAsync(cartId);
            }
            else
            {
                cart = await _context.Carts.Where(c => c.UserId == userId && c.IsActive)
                                           .FirstOrDefaultAsync();
            }
            if (cart != null)
            {
                CartItem item = await _context.CartItems.Where(x => x.ItemId == cartItem.ItemId &&
                x.CartId == cart.Id)
                    .FirstOrDefaultAsync();
                if (item != null)
                {
                    item.Quantity += cartItem.Quantity;
                    await _context.SaveChangesAsync();
                    return cart;
                }
                else
                {
                    cart.CartItems.Add(cartItem);
                    await _context.SaveChangesAsync();
                    return cart;
                }
            }
            else
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = true,
                };
                cart.CartItems.Add(cartItem);
                await _context.Carts.AddAsync(cart);
                await _context.SaveChangesAsync();
                return cart;
            }
        }

        public async Task<int> DeleteItemAsync(int itemId, int cartId)
        {
            return await _context.CartItems
                .Where(x=>x.ItemId == itemId && x.CartId == cartId)
                .ExecuteDeleteAsync();
        }

        public async Task<Cart> GetCartAsync(int cartId)
        {
            return await _context.Carts
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.Id == cartId && x.IsActive);
        }

        public async Task<int> GetCartItemCount(int userId)
        {
            Cart cart = await _context.Carts
                .Include(x=>x.CartItems)
                .Where(x=>x.UserId == userId && x.IsActive)
                .FirstOrDefaultAsync();
            if (cart != null)
            {
                return cart.CartItems.Sum(x => x.Quantity);
            }

            return 0;
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsAsync(int cartId)
        {
            return await _context.CartItems
                .Where(x=>x.CartId == cartId)
                .ToListAsync();
        }

        public async Task<Cart> GetUserCartAsync(int userId)
        {
            return await _context.Carts
                .Include(x=>x.CartItems)
                .Where(x=>x.UserId == userId && x.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> MakeInActiveAsync(int cartId)
        {
            Cart cart = await _context.Carts
                .Where(x=>x.Id == cartId && x.IsActive)
                .FirstOrDefaultAsync();

            if (cart != null)
            {
                cart.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<int> UpdateCartItem(int cartId, int itemId, int Quantity)
        {
            CartItem cart = await _context.CartItems
                .Where(x=>x.CartId == cartId && x.ItemId== itemId)
                .FirstOrDefaultAsync();
            if (cart != null)
            {
                cart.Quantity += Quantity;
                await _context.SaveChangesAsync();
                return 1;
            }
            return 0;
        }
    }
}
