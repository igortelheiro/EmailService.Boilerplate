using System;
using MGR.Common.Api;
using MGR.Common.Api.Configuration;
using MGR.EmailService.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MGR.EmailService.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.RegisterServices(Configuration);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CSN.EmailService.API", Version = "v1" });
                c.AddSecurityDefinition(Headers.ApiKey,
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Name = Headers.ApiKey,
                        Type = SecuritySchemeType.ApiKey
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = Headers.ApiKey }
                        },
                        Array.Empty<string>()
                    }
                });
            });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSN.EmailService.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseApiKeySecurity();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}