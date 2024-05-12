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
        public RabbitMqController(ILogger<RabbitMqController> logger,ISendMessageToQueue sendMessage)
        {
            _logger = logger;
            this.sendMessage = sendMessage;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] string message)
        {
            sendMessage.Send(message);
            return Ok("Message sent");
        }
    }
}
