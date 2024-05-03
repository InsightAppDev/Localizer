using System.Collections.Generic;
using System.Threading.Tasks;
using Lclzr.Providers;

namespace Lclzr.Tests.Infrastructure;

internal sealed class TestBlocksProvider : IBlocksProvider
{
    private readonly Block[] _blocks;

    public TestBlocksProvider(params Block[] blocks)
    {
        _blocks = blocks;
    }

    public Task<IReadOnlyCollection<Block>> GetBlocks()
    {
        return Task.FromResult<IReadOnlyCollection<Block>>(_blocks);
    }
}