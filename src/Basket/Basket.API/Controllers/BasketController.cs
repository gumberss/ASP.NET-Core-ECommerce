using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBusRabbitMq.Events;
using EventBusRabbitMq.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly IMapper _mapper;
        private readonly IBasketCheckoutPublisher _basketCheckoutPublisher;

        public BasketController(IBasketRepository repository, IMapper mapper, IBasketCheckoutPublisher basketCheckoutPublisher)
        {
            _repository = repository;
            _mapper = mapper;
            _basketCheckoutPublisher = basketCheckoutPublisher;
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string userName)
        {
            return Ok(await _repository.Get(userName) ?? new BasketCart(userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] BasketCart basket)
        {
            if (basket is null)
            {
                return BadRequest("The Basket to hold should not be null ");
            }

            return Ok(await _repository.Update(basket));
        }

        [HttpDelete("{userName}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Delete(string userName)
        {
            return Ok(await _repository.Delete(userName));
        }

        [HttpPost("[Action]")]
        [ProducesResponseType(typeof(BasketCheckout), (int)HttpStatusCode.Accepted)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _repository.Get(basketCheckout.UserName);

            if (basket is null)
            {
                return BadRequest("The basket was not found");
            }

            var basketRemoved = await _repository.Delete(basket.UserName);

            if (!basketRemoved)
            {
                return BadRequest("It was not possible to checkot the basket");
            }

            try
            {
                var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
                eventMessage.RequestId = Guid.NewGuid();
                eventMessage.TotalPrice = basket.TotalPrice;

                _basketCheckoutPublisher.Publish(eventMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _repository.Update(basket);
            }

            return Accepted(basketCheckout);
        }
    }
}
