
using Contract;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace OrderService
{
    public class CreateOrder : BackgroundService
    {
        IBus bus;
        public CreateOrder(IBus bus)
        {
            this.bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                await Task.Delay(1000);
                var order = new OrderCreated(Guid.NewGuid(), 100.0m);
                Console.WriteLine($"Order {order.OrderId} created.");
                try
                {
                    await bus.Publish(order);
                }
                catch (Exception ex) 
                { 
                
                }
                
                Console.WriteLine("OrderCreated event published.");
            }
        }
    }
}
