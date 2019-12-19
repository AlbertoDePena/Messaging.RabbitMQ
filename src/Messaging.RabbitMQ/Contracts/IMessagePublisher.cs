using Numaka.Messaging.RabbitMQ.Models;

namespace Numaka.Messaging.RabbitMQ.Contracts
{
    public interface IMessagePublisher
    {
        void PublishMessage(NewMessage message);
    }
}