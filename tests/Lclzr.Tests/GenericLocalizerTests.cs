using Lclzr.Providers.Files.RawFiles;
using Lclzr.Registries;
using Xunit;

namespace Lclzr.Tests;

public sealed class GenericLocalizerTests
{
    private readonly Localizer<GenericLocalizerTests> _localizer;

    private static readonly RawFilesBlocksProviderOptions Options = new()
    {
        Path = "Resources",
        ReadNestedFolders = true
    };

    public GenericLocalizerTests()
    {
        var provider = new RawFilesBlocksProvider(Options);
        var registry = new LocalizerRegistry(provider);
        registry.Initialize().GetAwaiter().GetResult();

        _localizer = new Localizer<GenericLocalizerTests>(registry);
        _localizer.CurrentCulture = new LocalizerCulture("ru-ru");
    }

    [Fact]
    public void Get_returns_value_based_on_generic_argument_name()
    {
        var value = _localizer.Get("test");
        Assert.Equal("Hello!", value);
    }
}