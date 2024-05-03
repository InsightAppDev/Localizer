using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Lclzr.Providers.Files.EmbeddedResources;
using Xunit;

namespace Lclzr.Tests.Providers;

public class EmbeddedResourceBlocksProviderTests
{
    [Fact]
    public async Task Initialize_with_one_language_in_file_creates_provider_with_single_block_named_embedded_and_two_cultures()
    {
        var fileBlocksProviderOptions = new EmbeddedResourcesBlocksProviderOptions()
        {
            Assemblies = new[] { Assembly.GetExecutingAssembly().FullName }
        };

        var provider = new EmbeddedResourcesBlocksProvider(fileBlocksProviderOptions);

        var blocks = await provider.GetBlocks();
        Assert.Single(blocks);
        var block = blocks.Single();
        
        Assert.Equal(2, block.AvailableCultures.Count);
        Assert.Contains("en-us", block.AvailableCultures, StringComparer.OrdinalIgnoreCase);
        Assert.Contains("ru-ru", block.AvailableCultures, StringComparer.OrdinalIgnoreCase);
    }
    
    [Fact]
    public async Task Initialize_with_multiple_languages_in_file_creates_provider_with_single_block_named_embedded_and_two_cultures()
    {
        var fileBlocksProviderOptions = new EmbeddedResourcesBlocksProviderOptions()
        {
            Assemblies = new[] { Assembly.GetExecutingAssembly().FullName }
        };

        var provider = new EmbeddedResourcesBlocksProvider(fileBlocksProviderOptions);

        var blocks = await provider.GetBlocks();
        Assert.Single(blocks);
        var block = blocks.Single();
        
        Assert.Equal(2, block.AvailableCultures.Count);
        Assert.Contains("en-us", block.AvailableCultures, StringComparer.OrdinalIgnoreCase);
        Assert.Contains("ru-ru", block.AvailableCultures, StringComparer.OrdinalIgnoreCase);
    }
}