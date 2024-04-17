using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Insight.Localizer.Extensions
{
    internal class LocalizerInitializerBackgroundService : BackgroundService
    {
        private readonly LocalizerOptions _options;

        public LocalizerInitializerBackgroundService(IOptions<LocalizerOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Localizer.Initialize(_options);
            return Task.CompletedTask;
        }
    }

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<ILocalizer, Localizer>();
            services.AddTransient(typeof(ILocalizer<>), typeof(Localizer<>));
            services.AddHostedService<LocalizerInitializerBackgroundService>();

            return services;
        }
    }
}