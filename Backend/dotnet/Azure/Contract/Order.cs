namespace Contract
{
    public class Order
    {
        public Guid OrderId { get; init; }

        public Guid CustomerId { get; init; }

        public string ProductId { get; init; } = string.Empty;

        public int Quantity { get; init; }

        public decimal TotalAmount { get; init; }

        public DateTime CreatedAtUtc { get; init; }
    }
}
