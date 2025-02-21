using PaymentService.Data.Entities;

namespace PaymentService.Data
{
    public class InMemoryData
    {
        private List<Payment> _payments;
        public InMemoryData()
        {
            _payments = new List<Payment>();
        }
        public List<Payment> Payments { get => _payments; }
    }
}
