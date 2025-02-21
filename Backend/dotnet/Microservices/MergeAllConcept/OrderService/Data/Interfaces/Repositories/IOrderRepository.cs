using OrderService.Data.Entities;
using OrderService.Dtos;

namespace OrderService.Data.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        public Order CreateOrder(OrderDto order);
        public Order? GetOrder(Guid id);
        public bool UpdateOrderStaus(Guid id,string status);
    }
}
