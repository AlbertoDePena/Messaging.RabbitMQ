namespace Numaka.Messaging.RabbitMQ.Models
{
    public class NewMessage : Message
    {
        public NewMessage(string type, string data, string routingKey) : base(type, data)
        {
            RoutingKey = routingKey ?? string.Empty;
        }

        public string RoutingKey { get; }
    }
}