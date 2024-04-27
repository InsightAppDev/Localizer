using System.Collections.Generic;

namespace Lclzr.Providers
{
    public interface IBlocksProvider
    {
        IReadOnlyCollection<Block> GetBlocks();
    }
}