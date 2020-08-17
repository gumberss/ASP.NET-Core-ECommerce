using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext context, ILoggerFactory loggerFactory, int retry = 0)
        {
            try
            {
                await context.Database.MigrateAsync();

                if (!await context.Orders.AnyAsync())
                {
                    await context.Orders.AddRangeAsync(GetPreConfiguratedData());
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retry < 3)
                {
                    var log = loggerFactory.CreateLogger<OrderContextSeed>();
                    log.LogError(ex, ex.Message);

                    await SeedAsync(context, loggerFactory, ++retry);
                }
            }
        }

        private static IEnumerable<Order> GetPreConfiguratedData()
        {
            return new List<Order>
            {
                new Order
                {
                    UserName = "Batman",
                    FirstName = "Sr",
                    LastName = "Wayne",
                    EmailAddress = "bat@man.com.br",
                    AddressLine = "Cave",
                    Country = "Country of Gothan"
                }
            };
        }
    }
}
