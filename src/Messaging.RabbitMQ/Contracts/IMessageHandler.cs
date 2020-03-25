using System;
using System.Threading.Tasks;
using Numaka.Messaging.RabbitMQ.Models;

namespace Numaka.Messaging.RabbitMQ.Contracts
{
    /// <summary>
    /// Message Handler Interface
    /// </summary>
    public interface IMessageHandler : IDisposable
    {
        /// <summary>
        /// Start handling messages
        /// </summary>
        /// <param name="handleMessageAsync"></param>
        void Handle(Func<Message, Task<bool>> handleMessageAsync);
    }
}