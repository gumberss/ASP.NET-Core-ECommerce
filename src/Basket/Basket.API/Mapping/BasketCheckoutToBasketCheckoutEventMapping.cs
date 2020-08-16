using AutoMapper;
using Basket.API.Entities;
using EventBusRabbitMq.Events;

namespace Basket.API.Mapping
{
    public class BasketCheckoutToBasketCheckoutEventMapping : Profile
    {
        public BasketCheckoutToBasketCheckoutEventMapping()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>();
        }
    }
}
