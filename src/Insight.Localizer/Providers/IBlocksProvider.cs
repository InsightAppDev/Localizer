using System.Collections.Generic;

namespace Insight.Localizer.Providers
{
    public interface IBlocksProvider
    {
        IReadOnlyCollection<Block> GetBlocks();
    }
}