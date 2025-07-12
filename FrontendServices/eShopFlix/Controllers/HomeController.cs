using eShopFlix.HttpClients;
using eShopFlix.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eShopFlix.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CatalogServiceClient _catalogServiceClient;

        public HomeController(ILogger<HomeController> logger, CatalogServiceClient catalogServiceClient)
        {
            _logger = logger;
            _catalogServiceClient = catalogServiceClient;
        }

        public async Task<IActionResult> IndexAsync()
        {
            // This action can be used to display a list of products or categories.
            // You can call your CatalogServiceClient here to fetch data and pass it to the view.
            try
            {
                var products = await _catalogServiceClient.GetAllProducts();
                return View(products);
            }
            catch (Exception ex)
            {
                // Handle exceptions, log errors, etc.
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var productDetails = await _catalogServiceClient.GetProductById(id); // Example product ID
                if (productDetails == null)
                {
                    return NotFound("Product not found.");
                }
                return Json(productDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product details for ID {ProductId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error while fetching product details.");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
