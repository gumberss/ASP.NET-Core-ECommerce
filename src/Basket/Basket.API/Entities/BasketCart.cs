using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class BasketCart
    {

        public String UserName { get; set; }

        public List<BasketCartItem> Items{ get; set; }

        public BasketCart()
        {
            Items = new List<BasketCartItem>();
        }

        public BasketCart(string userName) : this()
        {
            UserName = userName;
        }

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);
    }
}
