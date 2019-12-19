using System;
using System.Linq;
using Polly;
using RabbitMQ.Client;

namespace Numaka.Messaging.RabbitMQ
{
    public class MessagingBase
    {
        public const string MessageTypeHeader = "Messaging.Core.MessageTypeHeader";

        public readonly string[] SupportedExchangeTypes = new string[] { ExchangeType.Direct, ExchangeType.Fanout, ExchangeType.Topic };

        public void Execute(Action action) =>
            Policy
            .Handle<Exception>()
            .WaitAndRetry(9, _ => TimeSpan.FromSeconds(5), (ex, ts) => Console.WriteLine($"Error connecting to RabbitMQ. Retrying in 5 sec.\n\n{ex}\n"))
            .Execute(action);

        public void ValidateExchangeType(string exchangeType)
        {
            if (!SupportedExchangeTypes.Contains(exchangeType)) throw new InvalidOperationException("Exchange Type must be direct, fanout or topic");
        }
    }
}