using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Mappers;
using Ordering.Application.Responses;
using Ordering.Application.ResultHandlers;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, Result<OrderResponse, BusinessError>>
    {
        private readonly IOrderRepository _orderRepository;

        public CheckoutOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<OrderResponse, BusinessError>> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var order = OrderMapper.Mapper.Map<Order>(request);

            if (order is null)
            {
                return new BusinessError("The order can't be null");
            }

            var newOrder = await _orderRepository.AddAsync(order);

            await _orderRepository.SaveChangesAsync();

            var orderResponse = OrderMapper.Mapper.Map<OrderResponse>(newOrder);

            return orderResponse;
        }
    }
}
