namespace DataSync.DBChangeEmitterApp.Extensions
{
#pragma warning disable CS8618
    internal class RabbitMqConfiguration
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ClientProvidedName { get; set; }
        public QueueConfig Queue { get; set; }
    }
    internal class QueueConfig
    {
        private string _routingKeys;
        public string Name { get; set; }
        public bool IsDurable { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsAutoDelete { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKeysCommaSaperated
        {
            get => _routingKeys;
            set => _routingKeys = value; // Setter to set the value of _routingKeys
        }
        internal string[] RoutingKeys
        {
            get => string.IsNullOrEmpty(_routingKeys)
                ? Array.Empty<string>()
                : _routingKeys.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
    }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
}
