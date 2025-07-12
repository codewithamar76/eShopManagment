using MassTransit;
using OrderStateMachine.Database.Entities;
using OrderStateMachine.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderStateMachine.StateMachine
{
    public class OrderMachine : MassTransitStateMachine<OrderState>
    {
        public OrderMachine()
        {
            InstanceState(x => x.CurrentState);
            Event(() => OrderStarted, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderAccepted, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => OrderCanceled, x => x.CorrelateById(context => context.Message.OrderId));
            Event(() => PaymentCanceled, x => x.CorrelateById(context => context.Message.OrderId));

            InstanceState(X=>X.CurrentState);

            Initially(
                When(OrderStarted)
                    .Then(context =>
                    {
                        context.Saga.OrderCreationDateTime = DateTime.UtcNow;
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.PaymentId = context.Message.PaymentId;
                        context.Saga.Products = context.Message.Products;
                        context.Saga.CartId = context.Message.CartId;
                    })
                    .TransitionTo(Started)
                    .Publish(context=> new StockValidate(context.Saga)));

            During(Started,
                When(OrderAccepted)
                    .Then(context =>
                    {
                        context.Saga.OrderAcceptedDateTime = DateTime.UtcNow;
                    })
                    .TransitionTo(Accepted),
                When(OrderCanceled)
                    .Then(context =>
                    {
                        context.Saga.OrderCancelDateTime = DateTime.UtcNow;
                    })
                    .TransitionTo(Canceled),
                When(PaymentCanceled)
                    .Then(context =>
                    {
                        context.Saga.OrderCancelDateTime = DateTime.UtcNow;
                        context.Saga.PaymentId = context.Message.PaymentId;
                        context.Saga.Products = context.Message.Products;
                        context.Saga.CartId = context.Message.CartId;
                    })
                    .TransitionTo(Canceled));
        }
        public State Started { get; private set; }
        public State Accepted { get; private set; }
        public State Canceled { get; private set; }
        public Event<IOrderStarted> OrderStarted { get; private set; }
        public Event<IOrderAccepted> OrderAccepted { get; private set; }
        public Event<IOrderCanceled> OrderCanceled { get; private set; }
        public Event<IPaymentCanceled> PaymentCanceled { get; private set; }
    }
}
