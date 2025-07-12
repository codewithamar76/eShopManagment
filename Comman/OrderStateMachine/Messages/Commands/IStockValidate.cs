using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.Messages.Commands
{
    public interface IStockValidate
    {
        public Guid OrderId { get; }
        public string PaymentId { get; }
        public string Products { get; }
        public long CartId { get; }
    }
}
