using Contract;
using MassTransit;

namespace InventeryService.Consumers
{
    public class InventoryConsumer : IConsumer<UpdateInventory>
    {
        public async Task Consume(ConsumeContext<UpdateInventory> context)
        {
            Console.WriteLine($"Inventory updated for Order {context.Message.OrderId}");
            await context.Publish(new InventoryUpdated(context.Message.OrderId));
        }
    }
}
