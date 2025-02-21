using Contract;
using MassTransit;
using Orchestrator.States;

namespace Orchestrator
{
    public class OrderSaga : MassTransitStateMachine<OrderState>
    {
        public State ProcessingPayment { get; private set; }
        public State UpdatingInventory { get; private set; }
        public State CancelingOrder { get; private set; }
        public State CanceledOrder { get; private set; }
        public State Completed { get; private set; }

        public Event<OrderCreated> OrderCreated { get; private set; }
        public Event<PaymentProcessed> PaymentProcessed { get; private set; }
        public Event<PaymentFailed> PaymentFailed { get; private set; }
        public Event<InventoryUpdated> InventoryUpdated { get; private set; }
        public Event<OrderCanceled> OrderCanceled { get; private set; }

        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreated, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => PaymentProcessed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => PaymentFailed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => InventoryUpdated, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => OrderCanceled, x => x.CorrelateById(ctx => ctx.Message.OrderId));

            Initially(
                When(OrderCreated)
                    .Then(ctx =>
                    {
                        ctx.Saga.OrderId = ctx.Message.OrderId;
                        ctx.Saga.Amount = ctx.Message.Amount;
                    })
                    .Publish(ctx => new UpdateOrderStatus(ctx.Saga.OrderId, nameof(ProcessingPayment)))
                    .TransitionTo(ProcessingPayment)
            );

            During(ProcessingPayment,
                When(PaymentProcessed)
                    .Then(ctx => Console.WriteLine($"Payment processed for Order {ctx.Saga.OrderId}"))
                    .Publish(ctx => new UpdateOrderStatus(ctx.Saga.OrderId, nameof(UpdatingInventory)))
                    .Publish(ctx => new UpdateInventory(ctx.Saga.OrderId))
                    .TransitionTo(UpdatingInventory),

                When(PaymentFailed)
                    .Then(ctx => Console.WriteLine($"Payment failed for Order {ctx.Saga.OrderId}. Cancling Order."))
                    .Publish(ctx => new UpdateOrderStatus(ctx.Saga.OrderId, nameof(CancelingOrder)))
                    .Publish(ctx => new CancelOrder(ctx.Saga.OrderId))
                    .TransitionTo(CancelingOrder)
            );

            During(UpdatingInventory,
                When(InventoryUpdated)
                    .Publish(ctx => new UpdateOrderStatus(ctx.Saga.OrderId, nameof(Completed)))
                    .Then(ctx => Console.WriteLine($"Inventory updated for Order {ctx.Saga.OrderId}"))
                    .TransitionTo(Completed)
            );

            During(CancelingOrder,
                When(OrderCanceled)
                .Publish(ctx => new UpdateOrderStatus(ctx.Saga.OrderId, nameof(CanceledOrder)))
                .Then(ctx => Console.WriteLine($"Order Canceled for Order {ctx.Saga.OrderId}"))
                .TransitionTo(CanceledOrder)
            );
        }
    }
}
