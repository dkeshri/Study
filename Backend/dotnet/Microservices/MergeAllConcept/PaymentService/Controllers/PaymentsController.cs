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
        IPaymentRepository _paymentRepository;
        public PaymentsController(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
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
            return Ok(item);
        }
    }
}
