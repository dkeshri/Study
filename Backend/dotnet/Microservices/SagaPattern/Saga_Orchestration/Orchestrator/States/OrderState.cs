using MassTransit;

namespace Orchestrator.States
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; } = string.Empty;
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
    }
}
