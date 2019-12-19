using System;
using System.Linq;
using Polly;
using RabbitMQ.Client;

namespace Numaka.Messaging.RabbitMQ
{
    /// <summary>
    /// Messaging Base
    /// </summary>
    public class MessagingBase
    {
        /// <summary>
        /// Message type header
        /// </summary>
        public const string MessageTypeHeader = "Messaging.Core.MessageTypeHeader";

        /// <summary>
        /// Supported exchange types
        /// </summary>
        /// <value></value>
        public readonly string[] SupportedExchangeTypes = new string[] { ExchangeType.Direct, ExchangeType.Fanout, ExchangeType.Topic };

        /// <summary>
        /// Execute an action with a wait and retry policy.
        /// Retry 9 times waiting 5 seconds in between.
        /// </summary>
        /// <param name="action"></param>
        public void Execute(Action action) =>
            Policy
            .Handle<Exception>()
            .WaitAndRetry(9, _ => TimeSpan.FromSeconds(5), (ex, ts) => Console.WriteLine($"Error connecting to RabbitMQ. Retrying in 5 sec.\n\n{ex}\n"))
            .Execute(action);

        /// <summary>
        /// Validate exchange type againts supported list
        /// </summary>
        /// <param name="exchangeType">The exchange type</param>
        public void ValidateExchangeType(string exchangeType)
        {
            if (!SupportedExchangeTypes.Contains(exchangeType)) throw new InvalidOperationException("Exchange Type must be direct, fanout or topic");
        }
    }
}