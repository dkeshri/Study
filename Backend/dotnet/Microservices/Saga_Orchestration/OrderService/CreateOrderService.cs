
using Contract;
using MassTransit;

namespace OrderService
{
    public class CreateOrderService : BackgroundService
    {
        IBus bus;
        public CreateOrderService(IBus bus)
        {
            this.bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Publishing an OrderCreated event to start the process
                Task.Delay(2000).Wait();    
                var order = new OrderCreated(Guid.NewGuid(), 100.0m);
                await bus.Publish(order);
                Console.WriteLine($"OrderCreated event published with orderId: {order.OrderId}");
            }
        }
    }
}
