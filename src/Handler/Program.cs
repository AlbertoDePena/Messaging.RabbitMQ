using Numaka.Messaging.RabbitMQ;
using Numaka.Messaging.RabbitMQ.Contracts;
using Numaka.Messaging.RabbitMQ.Models;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Handler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var handler = GetMessageHandler())
                {
                    handler.Handle(HandleMessageAsync);

                    Console.WriteLine("Listening...");
                    Console.WriteLine("Press Enter to exit...");
                    Console.ReadKey();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static IMessageHandler GetMessageHandler()
        {
            return new MessageHandler(
                host: "localhost", user: "guest", password: "guest",
                exchange: "Customers.Exchange", exchangeType: "fanout", queue: "HandleCustomers.Queue", routingKey: "");
        }

        private static Task<bool> HandleMessageAsync(Message message)
        {
            if (message.Type != "Customer")
            {
                Console.WriteLine("Message type not supported");

                return Task.FromResult(false);
            }

            var customer = JsonConvert.DeserializeObject<Customer>(message.Data);

            Console.WriteLine($"Message: ({customer.Id}) {customer.Name} - {customer.Code}");

            return Task.FromResult(true);
        }
    }
}
