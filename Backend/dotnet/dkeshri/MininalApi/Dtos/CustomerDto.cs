using MininalApi.Entities;

namespace MininalApi.Dtos
{
    public class CustomerDto
    {
        public required string Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public Address? Address { get; set; }
    }
}
