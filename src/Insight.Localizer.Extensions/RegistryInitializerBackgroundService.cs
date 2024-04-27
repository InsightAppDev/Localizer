using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.Localizer.Infrastructure;
using Insight.Localizer.Registries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Insight.Localizer.Extensions
{
    internal class RegistryInitializerBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RegistryInitializerBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var registry = _serviceProvider.GetRequiredService<ILocalizerRegistry>();
            if (registry is IInitializable initializable)
            {
                return initializable.Initialize();
            }

            return Task.CompletedTask;
        }
    }
}