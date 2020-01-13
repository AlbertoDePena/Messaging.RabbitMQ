using System.Collections.Generic;
using Numaka.Messaging.RabbitMQ.Models;

namespace Numaka.Messaging.RabbitMQ.Contracts
{
    /// <summary>
    /// Message Publisher Interface
    /// </summary>
    public interface IMessagePublisher
    {
        /// <summary>
        /// Publish a message
        /// </summary>
        /// <param name="message"></param>
        void PublishMessage(NewMessage message);

        /// <summary>
        /// Publish a list of messages
        /// </summary>
        /// <param name="messages"></param>
        void PublishMessages(IEnumerable<NewMessage> messages);
    }
}