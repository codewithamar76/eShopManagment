using AutoMapper;
using Catalog.Application.DTO;
using Catalog.Application.RepoContract;
using Catalog.Application.ServiceContract;
using Catalog.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Catalog.Application.Services
{
    public class ProductServices:IProductServices
    {
        private readonly IProductRepository _prod;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private string _imageServer;
        public ProductServices(IProductRepository productRepository, IMapper mapper,
            IConfiguration configuration)
        {
            _prod = productRepository;
            _mapper = mapper;
            _config = configuration;
            _imageServer = _config["ImageServer"];
        }

        public async Task AddProductAsync(ProductDTO product)
        {
            Product product1 = _mapper.Map<Product>(product);
            await _prod.AddProductAsync(product1);
            await _prod.SaveProductAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            Product product = await _prod.GetProductByIdAsync(productId);
            if (product != null)
            {
                await _prod.DeleteProductAsync(productId);
                await _prod.SaveProductAsync();
            }
        }

        public IQueryable<ProductDTO> SearchByNameAsync(string search)
        {
            var products = _prod.SearchByNameAsync(search);
            if (products == null || !products.Any())
            {
                return null;
            }

            // Use a projection instead of modifying the entity directly  
            var projectedProducts = products.Select(p => new Product
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                ImageUrl = $"{_imageServer}{p.ImageUrl}",
                CategoryId = p.CategoryId,
                CreatedDate = p.CreatedDate,
                Category = p.Category
            });

            return _mapper.Map<IQueryable<ProductDTO>>(projectedProducts);
        }
        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _prod.GetAllProductsAsync();
            if (products == null || !products.Any())
            {
                return null;
            }

            // Use a projection instead of modifying the entity directly  
            var projectedProducts = products.Select(p => new Product
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                ImageUrl = $"{_imageServer}{p.ImageUrl}",
                CategoryId = p.CategoryId,
                CreatedDate = p.CreatedDate,
                Category = p.Category
            });

            return _mapper.Map<IEnumerable<ProductDTO>>(projectedProducts);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _prod.GetProductByIdAsync(productId);
            if (product == null)
            {
                return null;
            }
            product.ImageUrl = $"{_imageServer}{product.ImageUrl}";
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetProductsByIdsAsync(int[] Ids)
        {
            var products = await _prod.GetProductsByIdsAsync(Ids);
            if (products == null || !products.Any())
            {
                return Enumerable.Empty<ProductDTO>();
            }
            var projectedProducts = products.Select(p => new Product
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                UnitPrice = p.UnitPrice,
                ImageUrl = $"{_imageServer}{p.ImageUrl}",
                CategoryId = p.CategoryId,
                CreatedDate = p.CreatedDate,
                Category = p.Category
            });
            return _mapper.Map<IEnumerable<ProductDTO>>(projectedProducts);
        }

        public async Task UpdateProductAsync(ProductDTO product)
        {
            var product1 = _mapper.Map<Product>(product);
            var existingProduct = await _prod.GetProductByIdAsync(product1.ProductId);
            if (existingProduct != null)
            {
                await _prod.UpdateProductAsync(product1);
                await _prod.SaveProductAsync();
            }
        }
    }
}
