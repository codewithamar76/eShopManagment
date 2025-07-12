using Catalog.Application.DTO;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.ServiceContract
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDTO>> GetAllProductsAsync();
        Task<ProductDTO> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductDTO>> GetProductsByIdsAsync(int[] Ids);
        Task AddProductAsync(ProductDTO product);
        Task UpdateProductAsync(ProductDTO product);
        IQueryable<ProductDTO> SearchByNameAsync(string search);
        Task DeleteProductAsync(int productId);
    }
}
