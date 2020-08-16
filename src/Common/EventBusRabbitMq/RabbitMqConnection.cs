using System;
using System.Threading;
using EventBusRabbitMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EventBusRabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMqConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;

            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException)
            {
                Thread.Sleep(2000);

                _connection = _connectionFactory.CreateConnection();
            }

            return IsConnected;
        }

        public IModel CreteModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMq connection - You must connect with RabbitMq first!");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            _connection.Dispose();
        }
    }
}
