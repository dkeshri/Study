using Dkeshri.MessageQueue.Interfaces;
using IOTConcept.WebApi.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace IOTConcept.WebApi.Controllers
{
    public class RabbitMqController : IotConceptControllerBase
    {
        private readonly ILogger<RabbitMqController> _logger;
        private readonly IMessageSender sendMessage;
        public RabbitMqController(ILogger<RabbitMqController> logger, IMessageSender sendMessage)
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
        public IActionResult SendMessageToDirectExchange([FromBody] MessageDto messageDto)
        {
            string message = messageDto.Message ?? string.Empty;
            string routingkey = messageDto.RoutingKey ?? string.Empty;

            sendMessage.SendToExchange(message, routingkey);
            return Ok("Exchange Message sent");
        }
    }
}
