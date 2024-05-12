using MessageQueue.RabbitMq.Interfaces;
using MessageQueue.RabbitMq.Logic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace MessageQueue.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RabbitMqController : ControllerBase
    {

        private readonly ILogger<RabbitMqController> _logger;
        private readonly ISendMessage sendMessage;
        public RabbitMqController(ILogger<RabbitMqController> logger,ISendMessage sendMessage)
        {
            _logger = logger;
            this.sendMessage = sendMessage;
        }

        [HttpPost("sendToQueue")]
        public IActionResult SendMessageToQueue([FromBody] string message)
        {
            sendMessage.SendToQueue(message);
            return Ok("Message sent");
        }

        [HttpPost("sendToDirectExchange")]
        public IActionResult SendMessageToDirectExchange([FromBody] string message)
        {
            sendMessage.SendToExchange(message);
            return Ok("Message sent");
        }
    }
}
