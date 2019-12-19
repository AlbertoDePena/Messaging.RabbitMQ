namespace Numaka.Messaging.RabbitMQ.Models
{
    /// <summary>
    /// New Message
    /// </summary>
    public class NewMessage : Message
    {
        /// <summary>
        /// New Message constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        public NewMessage(string type, string data, string routingKey) : base(type, data)
        {
            RoutingKey = routingKey ?? string.Empty;
        }

        /// <summary>
        /// Message routing key
        /// </summary>
        public string RoutingKey { get; }
    }
}