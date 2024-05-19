using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Logic;
using IOTConcept.WebApi.Dtos;
using MessageQueue.RabbitMq.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IOTConcept.WebApi.Controllers
{
    public class InfluxDbController : IotConceptControllerBase
    {
        private readonly ILogger<InfluxDbController> _logger;
        private readonly IInfluxDbMessageProcessor influxDbMessageProcessor;
        public InfluxDbController(ILogger<InfluxDbController> logger, IInfluxDbMessageProcessor influxDbMessageProcessor)
        {
            _logger = logger;
            this.influxDbMessageProcessor = influxDbMessageProcessor;
        }

        [HttpPost("write")]
        public IActionResult WriteMessage(InfluxDto influxDto)
        {
            Random rnd = new Random();
            int anyNumber = rnd.Next(10000); // generate number between  0 and 10000

            
            Message message = new Message()
            {
                Tag = influxDto.Tag,
                Value = anyNumber
            };
            influxDbMessageProcessor.WriteMessage(message);
            return Ok("Message sent");
        }

        [HttpGet("read")]
        public async Task<IActionResult> ReadMessage()
        {


            await influxDbMessageProcessor.ReadMessage();
            Task.CompletedTask.Wait();
            return Ok("Read  Messsage");
        }
    }
}
