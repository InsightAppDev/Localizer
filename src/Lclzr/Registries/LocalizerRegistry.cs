using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lclzr.Exceptions;
using Lclzr.Infrastructure;
using Lclzr.Providers;

namespace Lclzr.Registries
{
    internal class LocalizerRegistry : ILocalizerRegistry, IInitializable
    {
        private Dictionary<string, Block> _blocks = new Dictionary<string, Block>();

        private IBlocksProvider[]? _blockProviders;

        private bool _initialized;

        public IReadOnlyCollection<string> AvailableBlockNames => new Lazy<IReadOnlyCollection<string>>(
                () => _blocks
                    .Select(x => x.Key)
                    .ToList()
                    .AsReadOnly())
            .Value;

        public LocalizerRegistry(IEnumerable<IBlocksProvider> blocksProviders) : this(blocksProviders.ToArray())
        {
        }

        public LocalizerRegistry(params IBlocksProvider[] blocksProviders)
        {
            _blockProviders = blocksProviders;
        }

        public async Task Initialize()
        {
            if (_initialized)
                throw new InvalidOperationException($"{nameof(LocalizerRegistry)} already initialized");

            if (_blockProviders == null)
                throw new InvalidOperationException("There is no block providers");

            foreach (var blocksProvider in _blockProviders)
            {
                if (blocksProvider is IInitializable initializable)
                    await initializable.Initialize();

                var currentProviderBlocks = blocksProvider.GetBlocks();
                foreach (var block in currentProviderBlocks)
                {
                    if (_blocks.ContainsKey(block.Name))
                    {
                        throw new InvalidOperationException(
                            $"Error occured while adding blocks from {blocksProvider.GetType().Name}. Block already exists.");
                    }

                    _blocks.Add(block.Name, block);
                }
            }

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
            _blocks.TryGetValue(name, out var value)
                ? value
                : throw new MissingBlockException($"Block `{name}` missing");
    }
}