using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lclzr.Providers
{
    public interface IBlocksProvider
    {
        Task<IReadOnlyCollection<Block>> GetBlocks();
    }
}