using OrderService.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OrderService.Service
{
    public class RabbitMqService
    {
        public void Publish(OrderEvent orderEvent)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            // Create queue
            channel.QueueDeclare(
                queue: "orderQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var message = JsonSerializer.Serialize(orderEvent);
            var body = Encoding.UTF8.GetBytes(message);

            // Send message
            channel.BasicPublish(
                exchange: "",
                routingKey: "orderQueue",
                basicProperties: null,
                body: body);

            Console.WriteLine("✅ Message Sent");
        }
    }
}
