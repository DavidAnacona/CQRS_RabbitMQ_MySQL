using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MySQLService.Services
{
    public class PacienteRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public PacienteRabbitMqService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar la cola en RabbitMQ
            _channel.QueueDeclare(
                queue: "PacienteQueue",
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
                routingKey: "PacienteQueue",
                basicProperties: null,
                body: body
            );
        }
    }
}
