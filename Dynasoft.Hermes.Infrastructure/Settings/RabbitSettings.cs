namespace Dynasoft.Hermes.Infrastructure.Settings
{
    public class RabbitSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public int PrefetchQuantity { get; set; }
        public int NumberOfConsumers { get; set; }
        public int NumberOfChannels { get; set; }
        public Publishers Publishers { get; set; }
    }

    public class Publishers
    {
        public string Name { get; set; }
        public string ExchangeName { get; set; }
        public string EventRoutingKeyPrefix { get; set; }
        public string AuditRoutingKeyPrefix { get; set; }
        public string DurableExchange { get; set; }
        public string ExchangeType { get; set; }

    }
}
