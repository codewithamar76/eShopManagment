using eShopFlix.Helper;
using eShopFlix.HttpClients;
using eShopFlix.Models;
using Microsoft.AspNetCore.Mvc;

namespace eShopFlix.Controllers
{
    public class CartController : BaseController
    {
        private readonly CartServiceClient _cart;
        public CartController(CartServiceClient cartService)
        {
            _cart = cartService;
        }
        public IActionResult Index()
        {
            if(CurrentUser == null)
            {
                return RedirectToAction("Login", "Account", new {area=""});
            }

            CartModel cartModel = _cart.GetUserCartAsync(CurrentUser.UserId).Result;
            return View(cartModel);
        }

        [Route("Cart/AddToCart/{id}/{unitPrice}/{quantity}")]
        public async Task<IActionResult> AddToCart(int id,decimal unitPrice, int quantity = 1)
        {
            // This action can be used to add a product to the cart.
            // You can call your CatalogServiceClient here to fetch product details if needed.
            try
            {
                CartItemModel cartItemModel = new CartItemModel
                {
                    ItemId = id,
                    UnitPrice = unitPrice,
                    Quantity = quantity
                };

                CartModel model = await _cart.AddToCartAsync(cartItemModel, CurrentUser.UserId);
                if (model != null)
                {
                    return Json(new
                    {
                        status = "success",
                        count = model.CartItems.Count
                    });
                }

                return Json(new
                {
                    status = "failed",
                    count = 0
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetCartCount()
        {
            // This action can be used to add a product to the cart.
            // You can call your CatalogServiceClient here to fetch product details if needed.
            if (CurrentUser != null)
            {
                var count = _cart.GetCartItemCountAsync(CurrentUser.UserId).Result;
                return Json(count);
            }

            return Json(0);
        }

        [Route("Cart/UpdateQuantity/{id}/{cartID}/{quantity}")]
        public IActionResult UpdateQuantity(int id, int cartID, int quantity)
        {
            // This action can be used to add a product to the cart.
            // You can call your CatalogServiceClient here to fetch product details if needed.
            try
            {
                int count = _cart.UpdateQuantityAsync(cartID, id, quantity).Result;
                return Json(count);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Route("Cart/DeleteItem/{id}/{cartID}")]
        public IActionResult DeleteItem(int id, int cartID)
        {
            // This action can be used to add a product to the cart.
            // You can call your CatalogServiceClient here to fetch product details if needed.
            try
            {
                int count = _cart.DeleteCartItemAsync(cartID, id).Result;
                return Json(count);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(AddressModel address)
        {
            if (ModelState.IsValid)
            {
                TempData.Set("Address", address);
                return RedirectToAction("Index", "Payment");
            }
            return View();
        }
    }
}
