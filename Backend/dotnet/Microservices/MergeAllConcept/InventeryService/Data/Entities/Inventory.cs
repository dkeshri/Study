namespace InventeryService.Data.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }
        public string Item { get; set; } = null!;
        public int Count { get; set; }

        public string Status { get; set; }
    }
}
