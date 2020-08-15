using System.Text.Json;
using System.Threading.Tasks;
using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IBasketContext _context;

        public BasketRepository(IBasketContext context)
        {
            _context = context;
        }

        public async Task<BasketCart> Get(string userName)
        {
            var basket = await _context.Redis.StringGetAsync(userName);

            if (basket.IsNullOrEmpty)
            {
                return null;
            }

            return JsonSerializer.Deserialize<BasketCart>(basket);
        }

        public async Task<BasketCart> Update(BasketCart basket)
        {
            var updated = await _context.Redis.StringSetAsync(basket.UserName, JsonSerializer.Serialize(basket));

            return updated ? await Get(basket.UserName) : null;

        }

        public async Task<bool> Delete(string username)
        {
            return await _context.Redis.KeyDeleteAsync(username);
        }
    }
}
