using System.Threading.Tasks;
using Lclzr.Providers.Files.RawFiles;
using Xunit;

namespace Lclzr.Tests.Providers;

public class RawFilesBlocksProviderTests
{
    [Fact]
    public async Task Initialize_with_one()
    {
        var fileBlocksProviderOptions = new RawFilesBlocksProviderOptions
        {
            Path = "Resources",
            ReadNestedFolders = true
        };

        var provider = new RawFilesBlocksProvider(fileBlocksProviderOptions);
        await provider.Initialize();
        
        var blocks = provider.GetBlocks();
        Assert.Equal(5, blocks.Count);
    }
}