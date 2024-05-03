using System;
using Lclzr.Registries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lclzr.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLocalizer(this IServiceCollection services,
            Action<LocalizerBuilder> builder)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var localizerBuilder = new LocalizerBuilder();
            builder.Invoke(localizerBuilder);

            foreach (var provider in localizerBuilder.Providers)
            {
                services.AddSingleton(provider);
            }

            services.TryAddSingleton<ILocalizerRegistry, LocalizerRegistry>();

            Func<IServiceProvider, object> factory = ctx =>
                localizerBuilder
                .WithRegistry(ctx.GetRequiredService<ILocalizerRegistry>())
                .Build();
            
            var descriptor = new ServiceDescriptor(typeof(ILocalizer), factory, ServiceLifetime.Scoped);
            services.TryAdd(descriptor);

            services.AddScoped(typeof(ILocalizer<>), typeof(Localizer<>));
            services.AddHostedService<RegistryInitializerBackgroundService>();

            return services;
        }
    }
}