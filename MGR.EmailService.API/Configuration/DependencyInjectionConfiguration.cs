using System.Net.Http;
using MGR.Common.Api.Configuration;
using MGR.Common.EventBus.Events;
using MGR.Common.EventBus.Interfaces;
using MGR.Common.MassTransitEventBus.Configuration;
using MGR.EmailService.Application;
using MGR.EmailService.Application.IntegrationHandler;
using MGR.EmailService.Application.Interface;
using MGR.EmailService.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MGR.EmailService.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddSingleton(_ => new HttpClient());
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IIntegrationEventHandler<EmailRequestEvent>, EmailRequestHandler>();
            
            // Security
            services.AddApiKeySwaggerSecurity(typeof(Program).Assembly);
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
            
            bus.Subscribe<EmailRequestEvent, EmailRequestHandler>();
        }
    }
}