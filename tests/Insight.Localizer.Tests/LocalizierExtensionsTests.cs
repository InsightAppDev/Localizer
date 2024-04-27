using System;
using System.Threading;
using System.Threading.Tasks;
using Insight.Localizer.Extensions;
using Insight.Localizer.Providers.Files.RawFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Insight.Localizer.Tests;

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
    public void AddLocalizer_registers_localizer_initializer()
    {
        var sp = BuildServiceProvider();
        var localizer = sp.GetRequiredService<ILocalizer<LocalizierExtensionsTests>>();

        Assert.NotNull(localizer);
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

        services.AddLocalizer();

        return services.BuildServiceProvider();
    }
}