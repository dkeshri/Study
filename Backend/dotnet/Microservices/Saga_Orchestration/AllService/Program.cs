// Program.cs - Saga Orchestration Setup
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Configure MassTransit with Saga
builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<OrderSaga, OrderState>().InMemoryRepository();
    x.AddConsumer<PaymentConsumer>();
    x.AddConsumer<InventoryConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();
app.Run();

// Saga State Definition
public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = string.Empty;
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}

// Saga State Machine
public class OrderSaga : MassTransitStateMachine<OrderState>
{
    public OrderSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => PaymentProcessed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => PaymentFailed, x => x.CorrelateById(ctx => ctx.Message.OrderId));
        Event(() => InventoryUpdated, x => x.CorrelateById(ctx => ctx.Message.OrderId));

        Initially(
            When(OrderCreated)
                .Then(ctx => Console.WriteLine($"Order {ctx.Instance.OrderId} created."))
                .Publish(ctx => new ProcessPayment(ctx.Instance.OrderId, ctx.Instance.Amount))
                .TransitionTo(ProcessingPayment)
        );

        During(ProcessingPayment,
            When(PaymentProcessed)
                .Then(ctx => Console.WriteLine($"Payment processed for Order {ctx.Instance.OrderId}"))
                .Publish(ctx => new UpdateInventory(ctx.Instance.OrderId))
                .TransitionTo(UpdatingInventory),

            When(PaymentFailed)
                .Then(ctx => Console.WriteLine($"Payment failed for Order {ctx.Instance.OrderId}. Rolling back."))
                .Publish(ctx => new RollbackOrder(ctx.Instance.OrderId))
                .TransitionTo(RolledBack)
        );

        During(UpdatingInventory,
            When(InventoryUpdated)
                .Then(ctx => Console.WriteLine($"Inventory updated for Order {ctx.Instance.OrderId}"))
                .TransitionTo(Completed)
        );
    }

    public State ProcessingPayment { get; private set; }
    public State UpdatingInventory { get; private set; }
    public State RolledBack { get; private set; }
    public State Completed { get; private set; }

    public Event<OrderCreated> OrderCreated { get; private set; }
    public Event<PaymentProcessed> PaymentProcessed { get; private set; }
    public Event<PaymentFailed> PaymentFailed { get; private set; }
    public Event<InventoryUpdated> InventoryUpdated { get; private set; }
}

// Messages
public record OrderCreated(Guid OrderId, decimal Amount);
public record ProcessPayment(Guid OrderId, decimal Amount);
public record PaymentProcessed(Guid OrderId);
public record PaymentFailed(Guid OrderId, string Reason);
public record UpdateInventory(Guid OrderId);
public record InventoryUpdated(Guid OrderId);
public record RollbackOrder(Guid OrderId);

// Consumers
public class PaymentConsumer : IConsumer<ProcessPayment>
{
    public async Task Consume(ConsumeContext<ProcessPayment> context)
    {
        var success = new Random().Next(2) == 0;
        if (success)
        {
            Console.WriteLine($"Payment successful for Order {context.Message.OrderId}");
            await context.Publish(new PaymentProcessed(context.Message.OrderId));
        }
        else
        {
            Console.WriteLine($"Payment failed for Order {context.Message.OrderId}");
            await context.Publish(new PaymentFailed(context.Message.OrderId, "Insufficient funds"));
        }
    }
}

public class InventoryConsumer : IConsumer<UpdateInventory>
{
    public async Task Consume(ConsumeContext<UpdateInventory> context)
    {
        Console.WriteLine($"Inventory updated for Order {context.Message.OrderId}");
        await context.Publish(new InventoryUpdated(context.Message.OrderId));
    }
}

public class PaymentFailedConsumer : IConsumer<RollbackOrder>
{
    public async Task Consume(ConsumeContext<RollbackOrder> context)
    {
        Console.WriteLine($"Rolling back order {context.Message.OrderId}");
    }
}

// Publishing an OrderCreated event to start the process
var bus = app.Services.GetRequiredService<IBus>();
await bus.Publish(new OrderCreated(Guid.NewGuid(), 100.0m));
Console.WriteLine("OrderCreated event published.");
