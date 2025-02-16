
using Contract;
using MassTransit;

namespace AllService
{
    public class OrderBackgroundService : BackgroundService
    {
        IBus bus;
        public OrderBackgroundService(IBus bus)
        {
            this.bus = bus;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string? amount = "0";

            while (!stoppingToken.IsCancellationRequested && !string.IsNullOrEmpty(amount))
            {
                // Publishing an OrderCreated event to start the process
                Console.WriteLine("Enter Order Amount: ");
                amount = Console.ReadLine();
                decimal amountDecimal = Convert.ToDecimal(amount);  
                var order = new OrderCreated(Guid.NewGuid(), amountDecimal);
                await bus.Publish(order);
                Console.WriteLine($"OrderCreated event published with orderId: {order.OrderId}");
                
            }
        }
    }
}
