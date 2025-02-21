using InventeryService.Data.Entities;

namespace InventeryService.Data
{
    public class InMemoryData
    {
        private List<Inventory> _inventory;
        public InMemoryData()
        {
            _inventory = new List<Inventory>();
        }
        public List<Inventory> Inventories { get => _inventory; }
    }
}
