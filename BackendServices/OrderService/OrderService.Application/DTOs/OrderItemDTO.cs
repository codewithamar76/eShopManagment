
namespace OrderService.Application.DTOs
{
    public class OrderItemDTO
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }

        public Guid OrderId { get; set; }

        public virtual OrderDTO Order { get; set; }
    }
}
