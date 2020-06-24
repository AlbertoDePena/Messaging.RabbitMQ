using System.Collections.Generic;
using Numaka.RabbitMQ.Infrastructure.Models;

namespace Numaka.RabbitMQ.Infrastructure
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