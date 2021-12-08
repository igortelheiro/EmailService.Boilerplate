using EmailService.Application;
using EmailService.Application.Interface;
using EmailService.Application.Model;
using EmailService.EventBusAdapter.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
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
            
            // Security
            services.ConfigureCors(configuration);

            // SmtpClient
            services.Configure<SmtpConfiguration>(configuration.GetSection(nameof(SmtpConfiguration)));
            services.ConfigureFluentEmail(configuration);

            // EventBus
            services.ConfigureEventBus(services.BuildServiceProvider, configuration);
        }
    }
}