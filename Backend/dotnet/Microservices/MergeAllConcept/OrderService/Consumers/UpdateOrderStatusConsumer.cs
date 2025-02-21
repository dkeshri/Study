using Contract;
using MassTransit;
using OrderService.Data.Interfaces.Repositories;

namespace OrderService.Consumers
{
    public class UpdateOrderStatusConsumer : IConsumer<UpdateOrderStatus>
    {
        IOrderRepository _orderRepository;
        public UpdateOrderStatusConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public Task Consume(ConsumeContext<UpdateOrderStatus> context)
        {
            Console.WriteLine($"Updating Order : {context.Message.OrderId} Status : {context.Message.Status}");
            _orderRepository.UpdateOrderStaus(context.Message.OrderId, nameof(UpdateOrderStatus));
            return Task.CompletedTask;
        }
    }
}
