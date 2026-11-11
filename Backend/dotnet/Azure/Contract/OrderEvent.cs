using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public class OrderEvent
    {
        public Guid OrderId { get; init; }

        public Guid CustomerId { get; init; }

        public string ProductId { get; init; } = string.Empty;

        public DateTime CreatedAtUtc { get; init; }
    }
}
