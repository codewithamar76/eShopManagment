using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.Messages.Events
{
    public interface IOrderAccepted
    {
        public Guid OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Products { get; set; }
        public long CartId { get; set; }
        public DateTime AcceptedDateTime { get; set; }
    }
}
