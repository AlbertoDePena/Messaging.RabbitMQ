using System;

namespace Numaka.Messaging.RabbitMQ.Models
{
    public class Message
    {
        public Message(string type, string data)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentNullException(nameof(data));

            Type = type;
            Data = data;
        }

        public string Type { get; }

        public string Data { get; }
    }
}