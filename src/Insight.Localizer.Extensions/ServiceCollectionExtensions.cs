using System;
using Insight.Localizer.Providers;
using Insight.Localizer.Registries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Insight.Localizer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<ILocalizerRegistry, LocalizerRegistry>();
            services.TryAddScoped<ILocalizer, Localizer>();
            services.AddTransient(typeof(ILocalizer<>), typeof(Localizer<>));
            services.AddHostedService<RegistryInitializerBackgroundService>();

            return services;
        }

        public static IServiceCollection AddLocalizerProvider<T>(this IServiceCollection services)
            where T : class, IBlocksProvider
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddSingleton<IBlocksProvider, T>();

            return services;
        }
    }
}