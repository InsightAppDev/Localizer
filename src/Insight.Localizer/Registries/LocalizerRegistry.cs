using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Insight.Localizer.Infrastructure;
using Insight.Localizer.Providers;

namespace Insight.Localizer.Registries
{
    public class LocalizerRegistry : ILocalizerRegistry, IInitializable
    {
        private Dictionary<string, Block> _blocks = new Dictionary<string, Block>();
        
        private IBlocksProvider[]? _blockProviders;
        
        private bool _initialized;

        public IReadOnlyCollection<string> AvailableBlockNames => new Lazy<IReadOnlyCollection<string>>(
                () => Blocks
                    .Select(x => x.Key)
                    .ToList()
                    .AsReadOnly())
            .Value;

        public IReadOnlyDictionary<string, Block> Blocks => new ReadOnlyDictionary<string, Block>(_blocks);

        public LocalizerRegistry(IEnumerable<IBlocksProvider> blocksProviders) : this(blocksProviders.ToArray())
        {
        }

        public LocalizerRegistry(params IBlocksProvider[] blocksProviders)
        {
            _blockProviders = blocksProviders;
        }

        // TODO: Think about thread safety
        public async Task Initialize()
        {
            if (_initialized)
                throw new InvalidOperationException($"{nameof(LocalizerRegistry)} already initialized");

            if (_blockProviders == null)
            {
                throw new InvalidOperationException("There is no block providers");
            }

            foreach (var blocksProvider in _blockProviders)
            {
                if (blocksProvider is IInitializable initializable)
                    await initializable.Initialize();

                var currentProviderBlocks = blocksProvider.GetBlocks();
                foreach (var block in currentProviderBlocks)
                {
                    if (_blocks.ContainsKey(block.Name))
                    {
                        // TODO: Write log
                        throw new InvalidOperationException(
                            $"Error occured while adding blocks from {blocksProvider.GetType().Name}. Block already added.");
                    }

                    _blocks.Add(block.Name, block);
                }
            }

            // Clear providers to collect them
            // TODO: Think how to refactor this
            _blockProviders = null;
            _initialized = true;
        }

        public string GetAny(string block, string key)
        {
            return this[block].Get(LocalizerConstants.AnyCultureKey, key);
        }

        public string GetByCulture(string culture, string block, string key)
        {
            return this[block].Get(culture, key);
        }

        private Block this[string name] =>
            Blocks.TryGetValue(name, out var value)
                ? value
                : throw new MissingBlockException($"Block `{name}` missing");
    }
}