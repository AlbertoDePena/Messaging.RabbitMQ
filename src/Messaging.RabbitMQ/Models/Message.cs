using System;

namespace Numaka.Messaging.RabbitMQ.Models
{
    /// <summary>
    /// Message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Message constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        public Message(string type, string data)
        {
            if (string.IsNullOrWhiteSpace(type)) throw new ArgumentNullException(nameof(type));
            if (string.IsNullOrWhiteSpace(data)) throw new ArgumentNullException(nameof(data));

            Type = type;
            Data = data;
        }

        /// <summary>
        /// Message type
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Message data
        /// </summary>
        public string Data { get; }
    }
}