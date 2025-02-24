using Contract.Data.Context;
using Microsoft.EntityFrameworkCore;
using PaymentService.Data.Entities;
using PaymentService.Data.Interfaces;
using PaymentService.Dtos;

namespace PaymentService.Data.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        protected IDataContext DataContext { get; }
        public DbSet<Payment> Payments => DataContext.DbContext.Set<Payment>();
        public PaymentRepository(IDataContext dataContext)
        {
            DataContext = dataContext;
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
                OrderId = paymentDto.OrderId,
                Status = paymentDto.IsPaymentCanceled ? "PaymentCanceled":"PaymentSucceeded",
            };

            Payments.Add(payment);
            DataContext.DbContext.SaveChanges();
            return payment;
        }

        public Payment? GetPayment(Guid id)
        {
            Payment? payment = Payments.AsNoTracking().FirstOrDefault(o => o.Id == id);
            return payment;
        }
    }
}
