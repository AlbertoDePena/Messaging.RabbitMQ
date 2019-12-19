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
        void Start(Func<Message, Task<bool>> handleMessageAsync);

        /// <summary>
        /// Stop handling messages
        /// </summary>
        void Stop();
    }
}