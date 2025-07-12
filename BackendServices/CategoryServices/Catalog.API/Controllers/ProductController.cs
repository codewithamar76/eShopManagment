using Catalog.Application.DTO;
using Catalog.Application.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices _productServices;
        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errors = string.Join(",",
                        ModelState.Values.SelectMany(e => e.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest($"Invalid product information \n {errors}");
                }
                if (product == null)
                {
                    return BadRequest("Product data is null.");
                }
                await _productServices.AddProductAsync(product);
                return Ok("Product added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding product: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductDTO product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errors = string.Join(",",
                        ModelState.Values.SelectMany(e => e.Errors)
                        .Select(e => e.ErrorMessage));
                    return BadRequest($"Invalid product information \n {errors}");
                }
                if (product == null)
                {
                    return BadRequest("Product data is null.");
                }
                await _productServices.UpdateProductAsync(product);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating product: {ex.Message}");
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                var product = await _productServices.GetProductByIdAsync(productId);
                if (product == null)
                {
                    return NotFound($"Product with ID {productId} not found.");
                }
                await _productServices.DeleteProductAsync(productId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting product: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetProductsByIds(int[] ids)
        {
            try
            {
                if (ids == null || ids.Length == 0)
                {
                    return BadRequest("No product IDs provided.");
                }
                var products = await _productServices.GetProductsByIdsAsync(ids);
                if (products == null || !products.Any())
                {
                    return NotFound("No products found for the provided IDs.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("search")]
        public IActionResult SearchProducts([FromQuery] string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return BadRequest("Search term cannot be empty.");
                }
                var products = _productServices.SearchByNameAsync(searchTerm);
                //var filteredProducts = products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                //                                           p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                if (!products.Any())
                {
                    return NotFound("No products found matching the search term.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
