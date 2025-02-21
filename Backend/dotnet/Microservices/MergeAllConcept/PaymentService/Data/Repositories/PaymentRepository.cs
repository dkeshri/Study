using PaymentService.Data.Entities;
using PaymentService.Data.Interfaces;
using PaymentService.Dtos;

namespace PaymentService.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private List<Payment> Payments { get;  }
        public PaymentRepository(InMemoryData inMemoryData)
        {
            Payments = inMemoryData.Payments;
        }

        public Payment ProcessPayment(PaymentDto paymentDto)
        {
            Payment payment = new Payment()
            {
                Id = Guid.NewGuid(),
                Amount = paymentDto.Amount,
                CardNumber = paymentDto.CardNumber,
                Name = paymentDto.Name,
                CVV = paymentDto.CVV,
                Status = "PaymentRequestGenerated",
            };

            Payments.Add(payment);
            return payment;
        }

        public Payment? GetPayment(Guid id)
        {
            Payment? payment = Payments.FirstOrDefault(o => o.Id == id);
            return payment;
        }
    }
}
