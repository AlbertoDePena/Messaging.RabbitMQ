using System;
using System.Linq;

namespace Numaka.RabbitMQ.Infrastructure
{
    /// <summary>
    /// Messaging Base
    /// </summary>
    public class MessagingBase
    {
        /// <summary>
        /// Message type
        /// </summary>
        public const string MessageType = "Numaka.RabbitMQ.Infrastructure.MessageType";

        /// <summary>
        /// Supported exchange types
        /// </summary>
        /// <value></value>
        public readonly string[] SupportedExchangeTypes = new string[] { "direct", "fanout", "topic" };

        /// <summary>
        /// Host
        /// </summary>
        public readonly string Host;

        /// <summary>
        /// User
        /// </summary>
        public readonly string User;

        /// <summary>
        /// Password
        /// </summary>
        protected readonly string Password;

        /// <summary>
        /// Exchange
        /// </summary>
        public readonly string Exchange;

        /// <summary>
        /// Exchange type
        /// </summary>
        public readonly string ExchangeType;

        /// <summary>
        /// Messaging base constructor
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public MessagingBase(string host, string user, string password, string exchange, string exchangeType)
        {
            Host = string.IsNullOrWhiteSpace(host) ?
                throw new ArgumentNullException(nameof(host)) : host;

            User = string.IsNullOrWhiteSpace(user) ?
                throw new ArgumentNullException(nameof(user)) : user;

            Password = string.IsNullOrWhiteSpace(password) ?
                throw new ArgumentNullException(nameof(password)) : password;

            Exchange = string.IsNullOrWhiteSpace(exchange) ?
                throw new ArgumentNullException(nameof(exchange)) : exchange;
                
            ExchangeType = string.IsNullOrWhiteSpace(exchangeType) ?
                throw new ArgumentNullException(nameof(exchangeType)) : exchangeType;

            if (!SupportedExchangeTypes.Contains(ExchangeType)) 
                throw new InvalidOperationException("Exchange Type must be direct, fanout or topic");
        }

        /// <summary>
        /// Execute an action with a wait and retry policy.
        /// Retry 9 times waiting 5 seconds in between.
        /// </summary>
        /// <param name="action"></param>
        public void Execute(Action action) => action();
            // Policy
            // .Handle<Exception>()
            // .WaitAndRetry(9, _ => TimeSpan.FromSeconds(5), (ex, ts) => Console.WriteLine($"Error connecting to RabbitMQ. Retrying in 5 sec.\n\n{ex}\n"))
            // .Execute(action);
    }
}