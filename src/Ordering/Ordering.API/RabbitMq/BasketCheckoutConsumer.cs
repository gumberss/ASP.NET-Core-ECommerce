using System;
using System.Text;
using System.Text.Json;
using AutoMapper;
using EventBusRabbitMq.Common;
using EventBusRabbitMq.Events;
using EventBusRabbitMq.Interfaces;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Ordering.API.RabbitMq
{
    public class BasketCheckoutConsumer
    {
        private readonly IRabbitMqConnection _connnection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;

        public BasketCheckoutConsumer(
            IRabbitMqConnection connnection
          , IMediator mediator
          , IMapper mapper
          , IOrderRepository repository)
        {
            _connnection = connnection;
            _mediator = mediator;
            _mapper = mapper;
            _repository = repository;
        }

        public void Consume()
        {
            var channel = _connnection.CreteModel();
            channel.ExchangeDeclare(EventBusConstants.BasketCheckoutExchange, "fanout", false, false, null);
            channel.QueueDeclare(EventBusQueueConstants.BasketCheckoutOrderQueue, false, false, false, null);
            channel.QueueBind(
                queue: EventBusQueueConstants.BasketCheckoutOrderQueue
              , exchange: EventBusConstants.BasketCheckoutExchange
              , routingKey: String.Empty
            );

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(
                queue: EventBusQueueConstants.BasketCheckoutOrderQueue
              , autoAck: true
              , consumer: consumer
            );
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);

            var eventData = JsonSerializer.Deserialize<BasketCheckoutEvent>(message);

            var command = _mapper.Map<CheckoutOrderCommand>(eventData);

            var result = await _mediator.Send(command);
        }

        public void Disconnect()
        {
            _connnection.Dispose();
        }
    }
}
