namespace MininalApi.Entities
{
    public class Customer : BaseEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Address? Address { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();

    }
}
