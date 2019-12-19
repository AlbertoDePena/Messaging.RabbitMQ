using System;
using System.Collections.Generic;
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
        private readonly string Host;

        private readonly string User;

        private readonly string Password;

        private readonly string Exchange;

        private readonly string ExchangeType;

        /// <summary>
        /// Message Publisher constructor
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public MessagePublisher(string host, string user, string password, string exchange, string exchangeType)
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

            ValidateExchangeType(ExchangeType);
        }

        /// <inheritdoc />
        public void PublishMessage(NewMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            Execute(() =>
            {
                var factory = new ConnectionFactory() { HostName = Host, UserName = User, Password = Password };

                using (var connection = factory.CreateConnection())
                using (var model = connection.CreateModel())
                {
                    model.ExchangeDeclare(Exchange, type: ExchangeType, durable: true, autoDelete: false);

                    var body = Encoding.UTF8.GetBytes(message.Data);
                    var properties = model.CreateBasicProperties();
                    
                    properties.Headers = new Dictionary<string, object> { { MessageTypeHeader, message.Type } };
                    model.BasicPublish(Exchange, message.RoutingKey, properties, body);
                }
            });
        }
    }
}