using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockService.Application.DTOs
{
    public class StockDTO
    {
        public long StockId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
