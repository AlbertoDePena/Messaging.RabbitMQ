using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Numaka.Messaging.RabbitMQ.Contracts;
using Numaka.Messaging.RabbitMQ.Models;
using RabbitMQ.Client;

namespace Numaka.Messaging.RabbitMQ
{
    /// <summary>
    /// Message Publisher
    /// </summary>
    public class MessagePublisher : MessagingBase, IMessagePublisher
    {
        /// <summary>
        /// Message Publisher constructor
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public MessagePublisher(string host, string user, string password, string exchange, string exchangeType)
            : base(host, user, password, exchange, exchangeType) { }

        /// <inheritdoc />
        public void PublishMessage(NewMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            Publish(new NewMessage[] { message });
        }

        /// <inheritdoc />
        public void PublishMessages(IEnumerable<NewMessage> messages)
        {
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            if (messages.Count() == 0) throw new InvalidOperationException("No messages to publish");

            Publish(messages);
        }

        private void Publish(IEnumerable<NewMessage> messages)
        {
            Execute(() =>
            {
                var factory = new ConnectionFactory() { HostName = Host, UserName = User, Password = Password };

                using (var connection = factory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    model.ExchangeDeclare(Exchange, type: ExchangeType, durable: true, autoDelete: false);

                    foreach (var message in messages)
                    {
                        var body = Encoding.UTF8.GetBytes(message.Data);
                        var properties = model.CreateBasicProperties();

                        properties.Headers = new Dictionary<string, object> { { MessageTypeHeader, message.Type } };
                        model.BasicPublish(Exchange, message.RoutingKey, properties, body);
                    }
                }
            });
        }
    }
}