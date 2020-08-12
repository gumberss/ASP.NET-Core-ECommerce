using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();

            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetPreconfiguredProducts());
            }
        }

        private static IEnumerable<Product> GetPreconfiguredProducts()
        {
            return new List<Product>
            {
                new Product
                {
                    Name = "Batman",
                    Category = "Hero",
                    Summary=  "Black",
                    ImageFile = "https://veja.abril.com.br/wp-content/uploads/2016/06/batman-o-cavaleiro-das-trevas-ressurge-20120710-16-original.jpeg?quality=70&strip=info&w=928",
                    Price = 100000000.21m
                },
                  new Product
                {
                    Name = "Cup",
                    Category = "Cup",
                    Summary =  "White",
                    ImageFile = "https://images-na.ssl-images-amazon.com/images/I/71jMZuJU42L._SL1500_.jpg",
                    Price = 1000.21m
                }
            };
        }
    }
}
