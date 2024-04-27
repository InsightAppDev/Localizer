using System.Collections.Generic;
using Lclzr.Providers;

namespace Lclzr.Tests.Infratructure;

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