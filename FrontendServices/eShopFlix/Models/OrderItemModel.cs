namespace eShopFlix.Models
{
    public class OrderItemModel
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal Total { get; set; }

        public Guid OrderId { get; set; }

        public virtual OrderModel Order { get; set; }
    }
}