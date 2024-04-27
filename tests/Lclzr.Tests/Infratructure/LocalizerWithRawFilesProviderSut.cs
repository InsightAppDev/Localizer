using Lclzr.Providers.Files.RawFiles;
using Lclzr.Registries;

namespace Lclzr.Tests.Infratructure;

public sealed class LocalizerWithRawFilesProviderSut
{
    public ILocalizer Localizer { get; }

    private static readonly RawFilesBlocksProviderOptions Options = new()
    {
        Path = "Resources",
        ReadNestedFolders = true
    };

    public LocalizerWithRawFilesProviderSut(RawFilesBlocksProviderOptions? options = null)
    {
        var provider = new RawFilesBlocksProvider(options ?? Options);
        var registry = new LocalizerRegistry(provider);
        Localizer = new Localizer(registry);
        registry.Initialize().GetAwaiter().GetResult();
    }
}