
using Contract;
using MassTransit;

namespace OrderService
{
    public class CreateOrder : BackgroundService
    {
        IBus bus;
        ILogger<CreateOrder> logger;
        public CreateOrder(IBus bus,ILogger<CreateOrder> logger)
        {
            this.bus = bus;
            this.logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (true)
            {
                await Task.Delay(1000);
                var order = new OrderCreated(Guid.NewGuid(), 100.0m);
                logger.LogInformation($"Order {order.OrderId} created.");
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
