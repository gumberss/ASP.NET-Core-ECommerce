using System.Threading.Tasks;
using Basket.API.Entities;

namespace Basket.API.Repositories.Interfaces
{
    public interface IBasketRepository
    {
        Task<BasketCart> Get(string userName);

        Task<BasketCart> Update(BasketCart basket);

        Task<bool> Delete(string username);
    }
}
