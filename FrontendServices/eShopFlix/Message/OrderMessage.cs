namespace eShopFlix.Message
{
    public class OrderMessage
    {
        public string PaymentId { get; set; }
        public long CartId { get; set; }
        public long UserId { get; set; }
        public string Products { get; set; }
    }
}
