using Microsoft.EntityFrameworkCore;
using StockService.Application.RepositoryContract;
using StockService.Domain.Entities; 

namespace StockService.Infrastructure.Persistence.RepositoryImplementation
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _context;
        public StockRepository(StockDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckStockAvailablity(int productId, int quantity)
        {
            return await _context.Stocks.AnyAsync(x => x.ProductId == productId && x.Quantity >= quantity);
        }

        public async Task<int> GetStockByProductIdAsync(int id)
        {
            return await _context.Stocks.Where(x => x.ProductId == id)
                 .Select(x => x.Quantity)
                 .FirstOrDefaultAsync();
        }

        public async Task<bool> ReserveStock(int productId, int quantity)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.ProductId == productId && x.Quantity >= quantity);

            if (stock != null)
            {
                stock.Quantity -= quantity;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateStock(int productId, int quantity)
        {
            try
            {
                var stocks = await _context.Stocks.Where(x => x.ProductId == productId).FirstOrDefaultAsync();
                if (stocks != null)
                {
                    stocks.Quantity += quantity;
                    await _context.SaveChangesAsync();
                    return true;
                }
                stocks = new Stock()
                {
                    Quantity = quantity,
                    ProductId = productId
                };
                await _context.Stocks.AddAsync(stocks);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) { 
                return false;
            }
        }
    }
}
