using System;
using System.Text;
using System.Text.Json;
using EventBusRabbitMq.Interfaces;
using RabbitMQ.Client;

namespace EventBusRabbitMq.Producer
{
    public abstract class RabbitMqProducer<T> where T : IRabbitEvent
    {
        private readonly IRabbitMqConnection _connection;

        protected abstract String Exchange { get; }

        protected RabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void Publish(T @event)
        {
            using (var channel = _connection.CreteModel())
            {
                channel.ExchangeDeclare(Exchange, "fanout", false, false, null);

                var message = JsonSerializer.Serialize(@event);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();

                properties.Persistent = false;
                properties.DeliveryMode = 2;

                channel.ConfirmSelect();
                channel.BasicPublish(Exchange, "", true, properties, body);

                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Message Sent");
                };

                channel.ConfirmSelect();
            }
        }
    }
}
