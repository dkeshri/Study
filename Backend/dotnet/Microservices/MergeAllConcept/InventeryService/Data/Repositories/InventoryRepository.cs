using InventeryService.Data.Entities;
using InventeryService.Data.Interfaces.Repositories;
using InventeryService.Dtos;

namespace InventeryService.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private List<Inventory> Inventories { get; }
        public InventoryRepository(InMemoryData inMemoryData)
        {
            Inventories = inMemoryData.Inventories;
        }
        public Inventory CreateInventory(InventoryDto inventoryDto)
        {
            Inventory inventory = new Inventory()
            {
                Id = Guid.NewGuid(),
                Item = inventoryDto.Item,
                Count = inventoryDto.Count,
                Status = "InventoryCreated",
            };

            Inventories.Add(inventory);
            return inventory;
        }

        public Inventory? GetInventory(Guid id)
        {
            Inventory? inventory = Inventories.FirstOrDefault(o => o.Id == id);
            return inventory;
        }
    }
}
