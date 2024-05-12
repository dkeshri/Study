namespace MessageQueue.WebApi.Dtos
{
    public class MessageDto
    {
        public string? Message { get; set; }
        public string? RoutingKey { get; set; }

    }
}
