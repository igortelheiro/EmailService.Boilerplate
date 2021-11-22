using System;
using System.Data.Common;
using EventBus.Core.Events;
using EventBus.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQEventBus.Configuration;

namespace EmailService.EventBusAdapter.Config
{
    public static class EventBusConfig
    {
        public static void ConfigureEventBus(this IServiceCollection services, Func<IServiceProvider> serviceProviderBuilder)
        {
            services.ConfigureSqlDbConnection();

            services.ConfigureRabbitMQEventBus();

            services.AddScoped<IIntegrationEventHandler<EmailRequestedEvent>, EmailRequestHandler>();

            var sp = serviceProviderBuilder.Invoke();
            var bus = sp.GetRequiredService<IEventBus>();
            bus.SubscribeHandlers();
        }


        private static void ConfigureSqlDbConnection(this IServiceCollection services) =>
            services.AddScoped<DbConnection>(sp => {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString(nameof(DbConnection));
                return new SqlConnection(connectionString);
            });


        private static void SubscribeHandlers(this IEventBus bus)
        {
            bus.Subscribe<EmailRequestedEvent, EmailRequestHandler>();
        }
    }
}
