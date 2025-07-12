using OrderStateMachine.Database.Entities;
using OrderStateMachine.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.StateMachine
{
    public class StockValidate : IStockValidate
    {
        private readonly OrderState _orderSaga;
        public StockValidate(OrderState orderSaga)
        {
            _orderSaga = orderSaga;
        }

        public Guid OrderId => _orderSaga.OrderId;
        public string PaymentId => _orderSaga.PaymentId;
        public string Products => _orderSaga.Products;
        public long CartId => _orderSaga.CartId;

    }
}
