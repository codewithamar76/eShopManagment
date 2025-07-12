namespace eShopFlix.Models
{
    public class CartModel
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsActive { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public List<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
    }
}
