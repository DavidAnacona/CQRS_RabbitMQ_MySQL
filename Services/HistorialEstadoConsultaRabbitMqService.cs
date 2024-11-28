using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MySQLService.Services
{
    public class HistorialEstadoConsultaRabbitMqService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public HistorialEstadoConsultaRabbitMqService()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Declarar la cola en RabbitMQ
            _channel.QueueDeclare(
                queue: "HistorialEstadoQueue",
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
                routingKey: "HistorialEstadoQueue",
                basicProperties: null,
                body: body
            );
        }
    }
}
