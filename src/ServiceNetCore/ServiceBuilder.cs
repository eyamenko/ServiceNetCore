using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceNetCore
{
    internal class ServiceBuilder : IServiceBuilder
    {
        private readonly ServiceCollection _services;

        internal ServiceBuilder()
        {
            _services = new ServiceCollection();

            AddWorkers();
            AddConfiguration();
        }

        public IServiceBuilder UseStartup<TStartup>() where TStartup : IStartup
        {
            _services.AddSingleton(typeof(IStartup), typeof(TStartup));

            var provider = _services.BuildServiceProvider();
            var startup = provider.GetService<IStartup>();

            startup.ConfigureServices(_services);

            return this;
        }

        public IService Build()
        {
            var provider = _services.BuildServiceProvider();

            return new Service(provider);
        }

        private void AddConfiguration()
        {
            var environment = Environment.GetEnvironmentVariable("SERVICENETCORE_ENVIRONMENT") ?? "Development";

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{environment}.json", true, true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            _services.AddSingleton<IConfiguration>(configuration);
        }

        private void AddWorkers()
        {
            var workerType = typeof(Worker);
            var workers = Assembly.GetEntryAssembly().DefinedTypes.Where(workerType.IsAssignableFrom);

            foreach (var worker in workers)
            {
                _services.AddTransient(workerType, worker);
            }
        }
    }
}