using System;
using EventBus.Core.Events;
using EventBus.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQEventBus.Configuration;

namespace EmailService.EventBusAdapter.Config
{
    public static class EventBusConfig
    {
        public static void ConfigureEventBus(this IServiceCollection services, Func<IServiceProvider> serviceProviderBuilder)
        {
            services.ConfigureRabbitMQEventBus();

            var sp = serviceProviderBuilder.Invoke();
            var bus = sp.GetRequiredService<IEventBus>();
            bus.SubscribeHandlers();
        }


        private static void SubscribeHandlers(this IEventBus bus)
        {
            bus.Subscribe<EmailRequestedEvent, EmailRequestHandler>();
        }
    }
}
