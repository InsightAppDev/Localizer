using System;
using System.Collections.Generic;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Lclzr.Extensions;
using Lclzr.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Lclzr.Tests;

public class LocalizierExtensionsTests
{
    [Fact]
    public void AddLocalizer_registers_non_generic_implementation()
    {
        var sp = BuildServiceProvider();
        var localizer = sp.GetRequiredService<ILocalizer>();

        Assert.NotNull(localizer);
    }

    [Fact]
    public void AddLocalizer_registers_generic_implementation()
    {
        var sp = BuildServiceProvider();
        var localizer = sp.GetRequiredService<ILocalizer<LocalizierExtensionsTests>>();

        Assert.NotNull(localizer);
    }

    [Fact]
    public void AddLocalizer_registers_registry_initializer()
    {
        var sp = BuildServiceProvider();
        var initializer = sp.GetRequiredService<IHostedService>();

        Assert.NotNull(initializer);
        Assert.Equal(typeof(RegistryInitializerBackgroundService), initializer.GetType());
    }

    [Fact]
    public async Task LocalizerInitializer_initializes_localizer()
    {
        var sp = BuildServiceProvider();
        var localizer = sp.GetRequiredService<ILocalizer<LocalizierExtensionsTests>>();

        var initializer = sp.GetRequiredService<IHostedService>();
        await initializer.StartAsync(CancellationToken.None);

        Assert.NotNull(localizer.AvailableBlockNames);
    }

    private static IServiceProvider BuildServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();

        var block = new Block("test");
        block.Add("ru-ru", new Dictionary<string, string>() );
        services.AddLocalizer(builder => builder.WithProvider(new TestBlocksProvider(block)));

        return services.BuildServiceProvider();
    }
}