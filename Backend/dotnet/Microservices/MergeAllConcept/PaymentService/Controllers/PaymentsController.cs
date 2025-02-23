using Contract;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Data.Entities;
using PaymentService.Data.Interfaces;
using PaymentService.Dtos;

namespace PaymentService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        IBus bus;
        IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentsController> _logger;
        public PaymentsController(ILogger<PaymentsController> logger, IPaymentRepository paymentRepository, IBus bus)
        {
            _logger = logger;
            _paymentRepository = paymentRepository;
            this.bus = bus;
        }

        [HttpGet]
        public ActionResult<Payment> Get(Guid id)
        {
            Payment? payment = _paymentRepository.GetPayment(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpPost]
        public ActionResult<Payment> ProcessPayment(PaymentDto paymentDto)
        {
            Payment item = _paymentRepository.ProcessPayment(paymentDto);
            var isPaymentCancled = paymentDto.IsPaymentCanceled;
            if (!isPaymentCancled)
            {
                bus.Publish(new PaymentProcessed(paymentDto.OrderId)).Wait();
                _logger.LogInformation($"PaymentProcessed event published with orderId: {paymentDto.OrderId}");
            }
            else
            {
                _logger.LogInformation($"Payment failed for Order {paymentDto.OrderId}");
                bus.Publish(new PaymentFailed(paymentDto.OrderId, "Insufficient funds")).Wait();
            }
            return Ok(item);
        }
    }
}
