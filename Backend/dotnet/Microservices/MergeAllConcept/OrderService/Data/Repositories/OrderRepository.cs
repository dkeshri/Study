using Contract.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OrderService.Data.Entities;
using OrderService.Data.Interfaces.Repositories;
using OrderService.Dtos;

namespace OrderService.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        protected IDataContext DataContext { get; }
        public DbSet<Order> Orders => DataContext.DbContext.Set<Order>();
        public OrderRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
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
            DataContext.DbContext.SaveChanges();
            return order;
        }

        public Order? GetOrder(Guid id)
        {
            Order? order = Orders.AsNoTracking().FirstOrDefault(o => o.Id == id);
            return order;
        }

        public bool UpdateOrderStaus(Guid id, string status)
        {
            var order = Orders.AsTracking().FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return false;
            }
            order.Status = status;
            DataContext.DbContext.SaveChanges();
            return true;
        }
    }
}
