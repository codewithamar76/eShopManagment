using Catalog.Application.RepoContract;
using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Persistence.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly CategoryDBContext _context;
        public ProductRepository(CategoryDBContext dBContext)
        {
            _context = dBContext;
        }

        public async Task AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public IQueryable<Product> SearchByNameAsync(string search)
        {
            return _context.Products
                .Include(c => c.Category)
                .Where(p => p.Name.Contains(search)) // Replace "searchTerm" with actual search criteria
                .AsQueryable();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products
                .Include(c => c.Category)
                .Where(p => p.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByIdsAsync(int[] Ids)
        {
            return await _context.Products
                .Where(p => Ids.Contains(p.ProductId))
                .ToListAsync();
        }

        public async Task<int> SaveProductAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
            if (existingProduct == null)
            {
                throw new ArgumentException($"Product with ID {product.ProductId} does not exist.");
            }
            _context.Products.Update(product);
        }
    }
}
