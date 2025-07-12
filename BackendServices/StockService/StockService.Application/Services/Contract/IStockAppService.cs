using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.Services.Contract
{
    public interface IStockAppService
    {
        Task<int> GetStockByProductIdAsync(int id);
        Task<bool> CheckStockAvailablity(int productId, int quantity);
        Task<bool> ReserveStock(int productId, int quantity);
        Task<bool> UpdateStock(int productId, int quantity);
    }
}
