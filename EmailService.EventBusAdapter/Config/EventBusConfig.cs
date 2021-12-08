using EventBus.Core.Events;
using EventBus.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQEventBus;
using System;
using System.Data.Common;

namespace EmailService.EventBusAdapter.Config
{
    public static class EventBusConfig
    {
        public static void ConfigureEventBus(this IServiceCollection services, Func<IServiceProvider> serviceProviderBuilder, IConfiguration configuration)
        {
            services.AddScoped<IIntegrationEventHandler<EmailRequestedEvent>, EmailRequestHandler>();

            services.AddScoped<DbConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString(nameof(DbConnection));
                return new SqlConnection(connectionString);
            });
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
