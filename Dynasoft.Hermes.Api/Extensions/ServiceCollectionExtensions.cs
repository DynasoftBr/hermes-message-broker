using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;

namespace Dynasoft.Hermes.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRabbitMq(this IServiceCollection services,
            string hostName,
            string userName,
            string password)
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = hostName,
                    UserName = userName,
                    Password = password
                };

                var connection = factory.CreateConnection();

                services.AddSingleton(connection);
            }
            catch (Exception e)
            {
                Log.Error("An error has occurred during the RabbitMQ connection initialize.", e);
            }
        }

        public static void SetUpSerilog(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(Enum.Parse<LogEventLevel>(config["Serilog:LogLevel:Hermes"]))
                .MinimumLevel.Override("Microsoft", Enum.Parse<LogEventLevel>(config["Serilog:LogLevel:Microsoft"]))
                .MinimumLevel.Override("System", Enum.Parse<LogEventLevel>(config["Serilog:LogLevel:System"]))
                .MinimumLevel.Override("Microsoft.AspNetCore.Server.Kestrel", Enum.Parse<LogEventLevel>(config["Serilog:LogLevel:Kestrel"]))
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
