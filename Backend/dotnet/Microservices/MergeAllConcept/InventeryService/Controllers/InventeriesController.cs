using InventeryService.Data.Entities;
using InventeryService.Data.Interfaces.Repositories;
using InventeryService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace InventeryService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventeriesController : ControllerBase
    {
        IInventoryRepository inventoryRepository;

        private readonly ILogger<InventeriesController> _logger;

        public InventeriesController(ILogger<InventeriesController> logger,IInventoryRepository inventoryRepository)
        {
            _logger = logger;
            this.inventoryRepository = inventoryRepository;
        }

        [HttpGet]
        public ActionResult<Inventory> Get(Guid id)
        {
            Inventory? inventory = inventoryRepository.GetInventory(id);
            if (inventory == null)
            {
                return NotFound();
            }
            return Ok(inventory);
        }

        [HttpPost]
        public ActionResult<Inventory> CreateInventory(InventoryDto inventoryDto) 
        {
            Inventory createdInventory = inventoryRepository.CreateInventory(inventoryDto);
            return Ok(createdInventory);
        }
    }
}
