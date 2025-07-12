using CartServices.Application.DTOs;
using CartServices.Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CartServices.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCart(int userId)
        {
            var cart = await _cartService.GetUserCartAsync(userId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }
        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCartItems(int cartId)
        {
            var items = await _cartService.GetCartItemsAsync(cartId);
            if (items == null || !items.Any())
            {
                return NotFound();
            }
            return Ok(items);
        }
        [HttpPost("{userId}")]
        public async Task<IActionResult> AddToCart(CartItemDTO cartItem, int userId)
        {
            var updatedCart = await _cartService.AddToCartAsync(cartItem, userId);
            if (updatedCart == null)
            {
                return BadRequest("Failed to add item to cart.");
            }
            return Ok(updatedCart);
        }
        [HttpDelete("{itemId}/{cartId}")]
        public async Task<IActionResult> DeleteItem(int itemId, int cartId)
        {
            var result = await _cartService.DeleteItemAsync(itemId, cartId);
            if (result <= 0)
            {
                return NotFound("Item not found in the cart.");
            }
            return Ok(result);
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartItemCount(int userId)
        {
            var count = await _cartService.GetCartItemCount(userId);
            return Ok(count);
        }
        
        [HttpGet("{CartId}/{ItemId}/{Quantity}")]
        public async Task<IActionResult> UpdateQuantity(int ItemId, int CartId, int Quantity)
        {
            var count = _cartService.UpdateCartItem(CartId, ItemId, Quantity).Result;
            return Ok(count);
        }

        [HttpGet("{CartId}")]
        public async Task<IActionResult> GetCart(int CartId)
        {
            var cart = await _cartService.GetCartAsync(CartId);
            return Ok(cart);
        }

        [HttpGet("{CartId}")]
        public IActionResult MakeInActive(int CartId)
        {
            var status = _cartService.MakeInActiveAsync(CartId);
            return Ok(status);
        }
    }
}
