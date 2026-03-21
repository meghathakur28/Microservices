using NotificationService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace NotificationService.Service
{
    public class RabbitMqConsumer
    {
        public void Start()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "orderQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var orderEvent = JsonSerializer.Deserialize<OrderEvent>(message);

                Console.WriteLine($"📩 Order Received: {orderEvent.OrderId}");
            };

            channel.BasicConsume(
                queue: "orderQueue",
                autoAck: true,
                consumer: consumer);

            Console.WriteLine("👂 Listening...");
        }
    }
}
