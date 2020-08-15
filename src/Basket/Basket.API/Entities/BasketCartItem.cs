using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.API.Entities
{
    public class BasketCartItem
    {
        public int Quantity { get; set; }

        public String Color { get; set; }

        public decimal Price { get; set; }

        public String ProductId { get; set; }

        public String ProductName { get; set; }
    }
}
