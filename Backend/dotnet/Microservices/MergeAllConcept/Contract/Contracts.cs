namespace Contract
{
    public record OrderCreated(Guid OrderId, decimal Amount);
    public record ProcessPayment(Guid OrderId, decimal Amount);
    public record PaymentProcessed(Guid OrderId);
    public record PaymentFailed(Guid OrderId, string Reason);
    public record UpdateInventory(Guid OrderId);
    public record InventoryUpdated(Guid OrderId);
    public record CancelOrder(Guid OrderId);
    public record OrderCanceled(Guid OrderId);
}
