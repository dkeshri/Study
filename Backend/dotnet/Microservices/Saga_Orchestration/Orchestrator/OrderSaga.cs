using Contract;
using MassTransit;
using Orchestrator.States;

namespace Orchestrator
{
    public class OrderSaga : MassTransitStateMachine<OrderState>
    {
        public State ProcessingPayment { get; private set; }
        public State UpdatingInventory { get; private set; }
        public State RolledBack { get; private set; }
        public State Completed { get; private set; }

        public Event<OrderCreated> OrderCreated { get; private set; }
        public Event<PaymentProcessed> PaymentProcessed { get; private set; }
        public Event<PaymentFailed> PaymentFailed { get; private set; }
        public Event<InventoryUpdated> InventoryUpdated { get; private set; }
        public OrderSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreated, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => PaymentProcessed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => PaymentFailed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
            Event(() => InventoryUpdated, x => x.CorrelateById(ctx => ctx.Message.OrderId));

            Initially(
                When(OrderCreated)
                    .Then(ctx =>
                    {
                        ctx.Saga.OrderId = ctx.Message.OrderId;
                        ctx.Saga.Amount = ctx.Message.Amount;
                    })
                    .Publish(ctx => new ProcessPayment(ctx.Saga.OrderId, ctx.Saga.Amount))
                    .TransitionTo(ProcessingPayment)
            );

            During(ProcessingPayment,
                When(PaymentProcessed)
                    .Then(ctx => Console.WriteLine($"Payment processed for Order {ctx.Saga.OrderId}"))
                    .Publish(ctx => new UpdateInventory(ctx.Saga.OrderId))
                    .TransitionTo(UpdatingInventory),

                When(PaymentFailed)
                    .Then(ctx => Console.WriteLine($"Payment failed for Order {ctx.Saga.OrderId}. Rolling back."))
                    .Publish(ctx => new RollbackOrder(ctx.Saga.OrderId))
                    .TransitionTo(RolledBack)
            );

            During(UpdatingInventory,
                When(InventoryUpdated)
                    .Then(ctx => Console.WriteLine($"Inventory updated for Order {ctx.Saga.OrderId}"))
                    .TransitionTo(Completed)
            );

            SetCompletedWhenFinalized();
        }
    }
}
