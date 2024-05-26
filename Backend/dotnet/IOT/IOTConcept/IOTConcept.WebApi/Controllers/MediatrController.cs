using IOTConcept.Influxdb.Interfaces;
using IOTConcept.Influxdb.Logic;
using IOTConcept.MediatR;
using IOTConcept.WebApi.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IOTConcept.WebApi.Controllers
{
    public class MediatrController : IotConceptControllerBase
    {
        IMediator mediator;
        public MediatrController(ILogger<InfluxDbController> logger,IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("Notification")]
        public IActionResult SendNotificationMessage(MediatrDto mediatrDto)
        {
            NotificationMessage notificationMessage = new NotificationMessage() { Message = mediatrDto.Message};
            mediator.Publish(notificationMessage);
            return Ok("Notification Message sent");
        }
    }
}
