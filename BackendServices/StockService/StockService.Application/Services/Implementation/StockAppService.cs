using StockService.Application.RepositoryContract;
using StockService.Application.Services.Contract;

namespace StockService.Application.Services.Implementation
{
    public class StockAppService : IStockAppService
    {
        private readonly IStockRepository _stock;
        public StockAppService(IStockRepository stock)
        {
            _stock = stock;
        }

        public async Task<bool> CheckStockAvailablity(int productId, int quantity)
        {
            return await _stock.CheckStockAvailablity(productId, quantity);
        }

        public async Task<int> GetStockByProductIdAsync(int id)
        {
            return await _stock.GetStockByProductIdAsync(id);
        }

        public async Task<bool> ReserveStock(int productId, int quantity)
        {
            return await _stock.ReserveStock(productId, quantity);
        }

        public async Task<bool> UpdateStock(int productId, int quantity)
        {
            return await _stock.UpdateStock(productId, quantity);
        }
    }
}
