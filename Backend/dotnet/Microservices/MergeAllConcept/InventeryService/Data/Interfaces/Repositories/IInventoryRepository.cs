using InventeryService.Data.Entities;
using InventeryService.Dtos;

namespace InventeryService.Data.Interfaces.Repositories
{
    public interface IInventoryRepository
    {
        public Inventory CreateInventory(InventoryDto inventory);
        public Inventory? GetInventory(Guid id);
    }
}
