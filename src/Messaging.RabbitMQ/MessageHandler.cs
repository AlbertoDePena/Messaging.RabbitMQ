using System;
using System.Text;
using System.Threading.Tasks;
using Numaka.Messaging.RabbitMQ.Contracts;
using Numaka.Messaging.RabbitMQ.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Numaka.Messaging.RabbitMQ
{
    /// <summary>
    /// Message Handler
    /// </summary>
    public class MessageHandler : MessagingBase, IMessageHandler
    {
        private readonly string Queue;

        private readonly string RoutingKey;

        private IConnection _connection;

        private IModel _model;

        private AsyncEventingBasicConsumer _consumer;

        private string _consumerTag;

        private Func<Message, Task<bool>> _handleMessageAsync;

        private bool _disposed;

        /// <summary>
        /// Message Handler constructor
        /// </summary>
        /// <param name="host"></param>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        public MessageHandler(string host, string user, string password, string exchange, string exchangeType, string queue, string routingKey = "")
            : base(host, user, password, exchange, exchangeType)
        {
            Queue = string.IsNullOrWhiteSpace(queue) ?
                throw new ArgumentNullException(nameof(queue)) : queue;

            RoutingKey = routingKey ?? string.Empty;
        }

        /// <inheritdoc />
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _model.BasicCancel(_consumerTag);
                _model.Close(200, "Closing connection...");
                _connection.Close();
            }

            _disposed = true;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Handle(Func<Message, Task<bool>> handleMessageAsync)
        {
            _handleMessageAsync = handleMessageAsync ??
                throw new ArgumentNullException(nameof(handleMessageAsync));

            Execute(() =>
            {
                var factory = new ConnectionFactory() { HostName = Host, UserName = User, Password = Password, DispatchConsumersAsync = true };

                _connection = factory.CreateConnection();

                _model = _connection.CreateModel();
                _model.ExchangeDeclare(Exchange, type: ExchangeType, durable: true, autoDelete: false);
                _model.QueueDeclare(Queue, durable: true, autoDelete: false, exclusive: false);
                _model.QueueBind(Queue, Exchange, RoutingKey);

                _consumer = new AsyncEventingBasicConsumer(_model);
                _consumer.Received += ConsumerReceivedAsync;
                _consumerTag = _model.BasicConsume(Queue, autoAck: false, _consumer);
            });
        }

        private async Task ConsumerReceivedAsync(object sender, BasicDeliverEventArgs args)
        {
            var type = string.Empty;

            if (args.BasicProperties.Headers.ContainsKey(MessageTypeHeader))
            {
                type = Encoding.UTF8.GetString((byte[])args.BasicProperties.Headers[MessageTypeHeader]);
            }

            var data = Encoding.UTF8.GetString(args.Body);

            var handled = await _handleMessageAsync(new Message(type, data));

            if (handled)
            {
                _model.BasicAck(args.DeliveryTag, multiple: false);
            }
        }
    }
}