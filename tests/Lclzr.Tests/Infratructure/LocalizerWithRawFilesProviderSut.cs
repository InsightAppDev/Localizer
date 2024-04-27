using Lclzr.Providers.Files.RawFiles;

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
        Localizer = new LocalizerBuilder()
            .WithRawFilesProvider(options ?? Options)
            .Build();
    }
}