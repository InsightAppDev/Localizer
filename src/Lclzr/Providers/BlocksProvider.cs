using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Lclzr.Providers
{
    internal abstract class BlocksProvider : IBlocksProvider
    {
        protected readonly IDictionary<string, Block> Blocks = new Dictionary<string, Block>();

        public virtual Task<IReadOnlyCollection<Block>> GetBlocks()
        {
            return Task.FromResult<IReadOnlyCollection<Block>>(Blocks.Values.ToArray());
        }

        protected Task InitializeBlockCulture(in BlockCultureData cultureData)
        {
            var blockName = cultureData.Block;
            var cultureString = cultureData.Culture;

            var json = cultureData.Content;
            var jObject = JObject.Parse(json);
            var blockContent = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var (key, value) in jObject)
                blockContent.Add(key, value.ToString());

            if (!Blocks.ContainsKey(blockName))
            {
                var block = new Block(blockName);
                Blocks.Add(block.Name, block);
            }

            Blocks[blockName].Add(cultureString, blockContent);
            return Task.CompletedTask;
        }
    }
}