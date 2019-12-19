using System;
using System.Threading.Tasks;
using Numaka.Messaging.RabbitMQ.Models;

namespace Numaka.Messaging.RabbitMQ.Contracts
{
    public interface IMessageHandler : IDisposable
    {
        void Start(Func<Message, Task<bool>> handleMessageAsync);

        void Stop();
    }
}