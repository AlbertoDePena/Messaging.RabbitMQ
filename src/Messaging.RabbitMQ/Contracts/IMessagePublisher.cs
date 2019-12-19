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
    }
}