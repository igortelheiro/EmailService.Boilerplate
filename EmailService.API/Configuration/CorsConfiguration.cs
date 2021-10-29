using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EmailService.API.Configuration
{
    public static class CorsConfiguration
    {
        public const string DefaultCorsPolicy = "DefaultCorsPolicy";
        
        public static IServiceCollection ConfigureCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicy, builder =>
                {
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                    builder.WithOrigins(configuration.GetValue<string>(CorsSection.OriginAddress));
                });
            });

            return services;
        }
    }
}