
using EventBusRabbitMq.Events;

namespace EventBusRabbitMq.Interfaces
{
    /// <summary>
    /// This was created because if someday you want to change RabbitMq, 
    /// just create another implementation of this interface that use
    /// (as example) kafka to publish messages, and change the concrete
    /// class injection in your Startup class.
    /// </summary>
    public interface IBasketCheckoutPublisher
    {
        void Publish(BasketCheckoutEvent @event);
    }
}
