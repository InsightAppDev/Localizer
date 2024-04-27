using System.Collections.Generic;
using Insight.Localizer.Providers;

namespace Insight.Localizer.Tests;

internal sealed class TestBlocksProvider : IBlocksProvider
{
    private readonly Block[] _blocks;

    public TestBlocksProvider(params Block[] blocks)
    {
        _blocks = blocks;
    }
    
    public IReadOnlyCollection<Block> GetBlocks()
    {
        return _blocks;
    }
}