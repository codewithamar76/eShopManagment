using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.Messages.Events
{
    public interface IStockValidated
    {
        public Guid OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Products { get; set; }
        public long CartId { get; set; }
    }
}
