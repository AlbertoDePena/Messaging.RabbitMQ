using System;
using System.Threading.Tasks;
using Numaka.RabbitMQ.Infrastructure.Models;

namespace Numaka.RabbitMQ.Infrastructure
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