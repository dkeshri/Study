namespace InventeryService.Extensions
{
    public static class ConfigurationExtensions
    {
        public static RabbitMqConfiguration? GetRabbitMqConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection("RabbitMq").Get<RabbitMqConfiguration>();
        }
    }
}
