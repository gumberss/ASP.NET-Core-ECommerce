using EventBusRabbitMq.Common;
using EventBusRabbitMq.Events;
using EventBusRabbitMq.Interfaces;

namespace EventBusRabbitMq.Producer
{
    public class BasketCheckoutPublisher : RabbitMqProducer<BasketCheckoutEvent>, IBasketCheckoutPublisher
    {
        public BasketCheckoutPublisher(IRabbitMqConnection connection) : base(connection)
        {
        }

        protected override string Exchange => EventBusConstants.BasketCheckoutExchange;
    }
}
