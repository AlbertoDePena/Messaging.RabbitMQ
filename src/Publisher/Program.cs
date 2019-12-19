using System;
using Numaka.Messaging.RabbitMQ;
using Numaka.Messaging.RabbitMQ.Contracts;
using Numaka.Messaging.RabbitMQ.Models;
using Newtonsoft.Json;

namespace Publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            var publisher = GetMessagePublisher();

            var key = PrintHelp().Key;

            while (key != ConsoleKey.Q)
            {

                if (key == ConsoleKey.N)
                {
                    Publish(publisher, GetCustomerName(), GetCustomerCode());
                }

                key = PrintHelp().Key;
            }
        }

        private static ConsoleKeyInfo PrintHelp()
        {
            Console.WriteLine("Publish Customer Message:");
            Console.WriteLine("N - New customer message");
            Console.WriteLine("Q - Quit application");
            Console.WriteLine();
            return Console.ReadKey();
        }

        private static string GetCustomerName()
        {
            Console.Write("Customer Name? ");
            return Console.ReadLine();
        }

        private static string GetCustomerCode()
        {
            Console.Write("Customer Code ?");
            return Console.ReadLine();
        }

        private static void Publish(IMessagePublisher publisher, string customerName, string customerCode)
        {
            Console.WriteLine();
            try
            {
                var customer = new Customer() { Id = Guid.NewGuid(), Name = customerName, Code = customerCode };

                var message = new NewMessage(type: "Customer", data: JsonConvert.SerializeObject(customer), routingKey: "");

                publisher.PublishMessage(message);

                Console.WriteLine($"Customer {customer.Name} has been published.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine();
        }

        private static IMessagePublisher GetMessagePublisher()
        {
            return new MessagePublisher(
                host: "localhost", user: "guest", password: "guest", exchange: "Customers.Exchange", exchangeType: "fanout");
        }
    }
}