global using System;
using EmailService.API.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((host, config) =>
{
    if (host.HostingEnvironment.IsDevelopment())
        config.MinimumLevel.Debug();

    config.MinimumLevel.Override("Microsoft", LogEventLevel.Information);
    config.Enrich.FromLogContext();
    config.WriteTo.Console();
});


// Dependency Injection
var services = builder.Services;

services.AddMvc();
services.AddSwaggerGen();
services.RegisterServices(builder.Configuration);


// Pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EmailService.API v1"));

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
    endpoints.MapControllers());


// Run
try
{
    Log.Information("Starting EmailService.Api");
    app.Run();
    Environment.Exit(0);
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    Environment.Exit(1);
}
finally
{
    Log.CloseAndFlush();
}