using OrderService.Data.Entities;
using OrderService.Data.Interfaces.Repositories;
using OrderService.Dtos;

namespace OrderService.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private List<Order> Orders { get; }
        public OrderRepository(InMemoryData inMemoryData)
        {
            Orders = inMemoryData.Orders;
        }

        public Order CreateOrder(OrderDto orderDto)
        {
            Order order = new Order()
            {
                Id = Guid.NewGuid(),
                Amount = orderDto.Amount,
                Status = "OrderCreated",
            };

            Orders.Add(order);
            return order;
        }

        public Order? GetOrder(Guid id)
        {
            Order? order = Orders.FirstOrDefault(o => o.Id == id);
            return order;
        }

        public bool UpdateOrderStaus(Guid id, string status)
        {
            var order = GetOrder(id);
            if (order == null)
            {
                return false;
            }
            order.Status = status;
            return true;
        }
    }
}
