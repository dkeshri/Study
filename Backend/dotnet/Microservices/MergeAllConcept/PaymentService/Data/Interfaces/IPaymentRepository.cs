using PaymentService.Data.Entities;
using PaymentService.Dtos;

namespace PaymentService.Data.Interfaces
{
    public interface IPaymentRepository
    {
        public Payment ProcessPayment(PaymentDto paymentDto);
        public Payment? GetPayment(Guid id);
    }
}
