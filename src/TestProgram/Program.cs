using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace TestProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices(ConfigureServices);

        /// <summary>
        /// Configure the worker services.
        /// </summary>
        /// <param name="hostContext">Host builder context.</param>
        /// <param name="services">Service collection to configure.</param>
        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddHostedService<MassTransitHostedService>();
            services.AddMassTransit(serviceConfigurator =>
            {
                serviceConfigurator.AddBus(serviceProvider =>
                {
                    var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                    LogContext.ConfigureCurrentLogContext(loggerFactory);

                    var bus = Bus.Factory.CreateUsingRabbitMq(c =>
                    {
                        c.Host(new Uri("rabbitmq://guest:guestX@localhost"));
                        c.ReceiveEndpoint("test", e =>
                        {
                            e.Consumer<TestConsumer>(serviceProvider);
                        });
                    });

                    //bus.ConnectReceiveEndpointObserver(new ReceiveEndpointObserver(loggerFactory.CreateLogger<ReceiveEndpointObserver>()));

                    return bus;
                });
            });
        }

        private class ReceiveEndpointObserver : IReceiveEndpointObserver
        {
            private readonly ILogger _logger;

            public ReceiveEndpointObserver(ILogger logger)
            {
                _logger = logger;
            }

            public Task Completed(ReceiveEndpointCompleted completed)
            {
                _logger.LogWarning("Faulted");
                return Task.CompletedTask;
            }

            public Task Faulted(ReceiveEndpointFaulted faulted)
            {
                _logger.LogWarning("Faulted");
                return Task.CompletedTask;
            }

            public Task Ready(ReceiveEndpointReady ready)
            {
                _logger.LogWarning("Ready");
                return Task.CompletedTask;
            }

            public Task Stopping(ReceiveEndpointStopping stopping)
            {
                _logger.LogWarning("Stopping");
                return Task.CompletedTask;
            }
        }
    }
}
