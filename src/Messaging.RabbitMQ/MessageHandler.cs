using System;
using System.Text;
using System.Threading.Tasks;
using Numaka.Messaging.RabbitMQ.Contracts;
using Numaka.Messaging.RabbitMQ.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Numaka.Messaging.RabbitMQ
{
    public class MessageHandler : MessagingBase, IMessageHandler
    {
        private readonly string Host;

        private readonly string User;

        private readonly string Password;

        private readonly string Exchange;

        private readonly string ExchangeType;

        private readonly string Queue;

        private readonly string RoutingKey;

        private IConnection _connection;

        private IModel _model;

        private AsyncEventingBasicConsumer _consumer;

        private string _consumerTag;

        private Func<Message, Task<bool>> _handleMessageAsync;

        private bool _disposed;

        public MessageHandler(string host, string user, string password, string exchange, string exchangeType, string queue, string routingKey = "")
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
            Queue = string.IsNullOrWhiteSpace(queue) ?
                throw new ArgumentNullException(nameof(queue)) : queue;

            RoutingKey = routingKey ?? string.Empty;

            ValidateExchangeType(ExchangeType);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Stop();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Start(Func<Message, Task<bool>> handleMessageAsync)
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

        public void Stop()
        {
            _model.BasicCancel(_consumerTag);
            _model.Close(200, "Closing connection...");
            _connection.Close();
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