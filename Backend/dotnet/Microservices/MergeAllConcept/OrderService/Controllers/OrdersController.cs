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
        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
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
            Order createdOrder= _orderRepository.CreateOrder(order);
            return Ok(createdOrder);
        }
    }
}
