namespace OrderService.API.Messages
{
    //Message broker like AZ Service Bus , RabbitMQ, etc. will use this class to send messages
    //It help to implement communication(pattern) between microservices
    public class OrderMessage
    {
        public string PaymentId { get; set; }
        public long CartId { get; set; }
        public long UserId { get; set; }
        public string Products { get; set; }
    }
}
