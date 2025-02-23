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
        public DbSet<Order> OrderEntities => DataContext.DbContext.Set<Order>();
        private List<Order> Orders { get; }
        public OrderRepository(IDataContext dataContext, InMemoryData inMemoryData)
        {
            Orders = inMemoryData.Orders;
            DataContext = dataContext;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            
            return OrderEntities.AsNoTracking().ToList();
           
        }
        public Order GetOrderById(Guid id)
        {
            return OrderEntities.Where(x => x.Id.Equals(id)).AsNoTracking()
                .FirstOrDefault();
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
