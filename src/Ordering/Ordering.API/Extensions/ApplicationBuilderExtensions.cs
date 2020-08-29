using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Ordering.API.RabbitMq;
using Microsoft.Extensions.Hosting;

namespace Ordering.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static List<BasketCheckoutConsumer> _consumers = new List<BasketCheckoutConsumer>();

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            _consumers.Add(app.ApplicationServices.GetService<BasketCheckoutConsumer>());

            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnStarted);
            lifetime.ApplicationStopping.Register(OnStopping);

            return app;
        }

        private static void OnStarted()
        {
            _consumers.ForEach(x => x.Consume());
        }

        private static void OnStopping()
        {
            _consumers.ForEach(x => x.Disconnect());
            _consumers.Clear();
        }
    }
}
