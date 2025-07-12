

namespace OrderService.Application.DTOs
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }

        public string PaymentId { get; set; }

        public string Street { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Locality { get; set; }

        public string PhoneNumber { get; set; }

        public long UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? AcceptDate { get; set; }

        public virtual ICollection<OrderItemDTO> OrderItems { get; set; } = new List<OrderItemDTO>();
    }
}
