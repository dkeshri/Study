using Contract.Data.Context;
using InventeryService.Data.Entities;
using InventeryService.Data.Interfaces.Repositories;
using InventeryService.Dtos;
using Microsoft.EntityFrameworkCore;

namespace InventeryService.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        protected IDataContext DataContext { get; }
        public DbSet<Inventory> Inventories => DataContext.DbContext.Set<Inventory>();
        public InventoryRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
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
            DataContext.DbContext.SaveChanges();
            return inventory;
        }

        public Inventory? GetInventory(Guid id)
        {
            Inventory? inventory = Inventories.AsNoTracking().FirstOrDefault(o => o.Id == id);
            return inventory;
        }
    }
}
