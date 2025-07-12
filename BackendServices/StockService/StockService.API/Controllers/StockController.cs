using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockService.Application.Services.Contract;

namespace StockService.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockAppService _stockAppService;
        public StockController(IStockAppService stockAppService)
        {
            _stockAppService = stockAppService;
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetStockByProductIdAsync(int productId)
        {
            var stock = await _stockAppService.GetStockByProductIdAsync(productId);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock);
        }
        [HttpPost]
        public async Task<IActionResult> ReserveStockAsync(int productId, int quantity)
        {
            var result = await _stockAppService.ReserveStock(productId, quantity);
            if (!result)
            {
                return BadRequest("Insufficient stock available.");
            }
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateStockAsync(int productId, int quantity)
        {
            var result = await _stockAppService.UpdateStock(productId, quantity);
            if (!result)
            {
                return BadRequest("Failed to update stock.");
            }
            return Ok(result);
        }
        [HttpGet()]
        public async Task<IActionResult> CheckAvailabilityAsync(int productId, int quantity)
        {
            var isAvailable = await _stockAppService.CheckStockAvailablity(productId, quantity);
            if (!isAvailable)
            {
                return BadRequest("Insufficient stock available.");
            }
            return Ok(isAvailable);
        }
    }
}
