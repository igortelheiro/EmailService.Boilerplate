using System.Net;
using System.Net.Mail;
using System.Text;
using EmailService.Application.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.API.Configuration
{
    public static class FluentEmailConfiguration
    {
        public static IServiceCollection ConfigureFluentEmail(this IServiceCollection services, IConfiguration configuration)
        {
            var smtpConfig = configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
            
            services
                .AddFluentEmail(smtpConfig.SenderEmail, smtpConfig.SenderName)
                .AddRazorRenderer()
                .AddSmtpSender(new SmtpClient(smtpConfig.Host)
                {
                    EnableSsl = smtpConfig.EnableSsl,
                    Port = smtpConfig.Port,
                    Credentials = new NetworkCredential(smtpConfig.SenderEmail, GetDecodedPassword(smtpConfig.Password))
                });

            return services;
        }
        
        
        private static string GetDecodedPassword(string password)
        {
            var bytes = Convert.FromBase64String(password);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}