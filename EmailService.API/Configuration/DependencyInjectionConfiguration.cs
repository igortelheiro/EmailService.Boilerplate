using EmailService.Application;
using EmailService.Application.IntegrationHandler;
using EmailService.Application.Interface;
using EmailService.Application.Model;
using EventBus.Core.Events;
using EventBus.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQEventBus.Configuration;
using System.Net.Http;

namespace EmailService.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddSingleton(_ => new HttpClient());
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IIntegrationEventHandler<EmailRequestedEvent>, EmailRequestHandler>();
            
            // Security
            services.ConfigureCors(configuration);

            // SmtpClient
            services.Configure<SmtpConfiguration>(configuration.GetSection(nameof(SmtpConfiguration)));
            services.ConfigureFluentEmail(configuration);

            // EventBus
            services.ConfigureRabbitMQEventBus();
            services.SubscribeHandlers();
        }


        private static void SubscribeHandlers(this IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            var bus = sp.GetRequiredService<IEventBus>();
            
            bus.Subscribe<EmailRequestedEvent, EmailRequestHandler>();
        }
    }
}