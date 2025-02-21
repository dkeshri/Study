using Contract;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Entities;
using OrderService.Data.Interfaces.Repositories;
using OrderService.Dtos;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {

        IOrderRepository _orderRepository;
        IBus bus;
        private readonly ILogger<OrdersController> _logger;
        public OrdersController(ILogger<OrdersController> logger, IOrderRepository orderRepository,IBus bus)
        {
            _logger = logger;
            _orderRepository = orderRepository;
            this.bus = bus; 
        }

        [HttpGet]
        public ActionResult<Order> Get(Guid id)
        {
            Order? order = _orderRepository.GetOrder(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public ActionResult<Order> CreateOrder(OrderDto order)
        {
            Order createdOrder = _orderRepository.CreateOrder(order);
            var orderCreatedEvent = new OrderCreated(createdOrder.Id, createdOrder.Amount);
            bus.Publish(orderCreatedEvent).Wait();
            _logger.LogInformation($"OrderCreated event published with orderId: {orderCreatedEvent.OrderId}");
            return Ok(createdOrder);
        }
    }
}
