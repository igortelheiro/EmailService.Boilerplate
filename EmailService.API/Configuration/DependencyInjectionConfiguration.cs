using EmailService.Application;
using EmailService.Application.Interface;
using EmailService.Application.Model;
using EventBus.Core.Events;
using EventBus.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
//using EmailService.EventBusAdapter.Config;

namespace EmailService.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddSingleton(_ => new HttpClient());
            services.AddSingleton<IEmailSenderService, EmailSenderService>();
            
            // Security
            services.ConfigureCors(configuration);

            // SmtpClient
            services.Configure<SmtpConfiguration>(configuration.GetSection(nameof(SmtpConfiguration)));
            services.ConfigureFluentEmail(configuration);

            // EventBus
            //services.ConfigureEventBus(services.BuildServiceProvider);
        }
    }
}