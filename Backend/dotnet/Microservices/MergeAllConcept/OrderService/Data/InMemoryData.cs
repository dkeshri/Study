using OrderService.Data.Entities;

namespace OrderService.Data
{
    public class InMemoryData
    {
        private List<Order> _orders;
        public InMemoryData()
        {
            _orders = new List<Order>();
        }
        public List<Order> Orders { get => _orders; }
    }
}
