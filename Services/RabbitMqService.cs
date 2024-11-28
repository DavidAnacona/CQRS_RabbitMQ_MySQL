using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CQRS_Rabbit_MySQL.Services
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            // Crear conexión de manera síncrona
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar la cola en RabbitMQ
            _channel.QueueDeclare(
                queue: "ConsultaQueue",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );
        }

        public void PublishMessage(object message)
        {
            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            // Publicar el mensaje en la cola
            _channel.BasicPublish(
                exchange: "",
                routingKey: "ConsultaQueue",
                basicProperties: null,
                body: body
            );
        }
    }
}
