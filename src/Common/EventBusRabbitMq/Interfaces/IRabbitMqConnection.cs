using System;
using RabbitMQ.Client;

namespace EventBusRabbitMq.Interfaces
{
    public interface IRabbitMqConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreteModel();
    }
}
