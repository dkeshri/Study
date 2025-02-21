using MassTransit;
using Contract;
using OrderService.Data.Interfaces.Repositories;

namespace OrderService.Consumers
{
    public class PaymentFailedConsumer: IConsumer<CancelOrder>
    {
        IOrderRepository _orderRepository;
        public PaymentFailedConsumer(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task Consume(ConsumeContext<CancelOrder> context)
        {
            Console.WriteLine($"cancelling order {context.Message.OrderId}");
            _orderRepository.UpdateOrderStaus(context.Message.OrderId, nameof(CancelOrder));
            Console.WriteLine($"OrderCanceled orderId : {context.Message.OrderId}");
            await context.Publish(new OrderCanceled(context.Message.OrderId));
        }
    }
}
