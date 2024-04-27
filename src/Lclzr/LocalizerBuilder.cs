using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lclzr.Providers;
using Lclzr.Providers.Files.EmbeddedResources;
using Lclzr.Providers.Files.RawFiles;
using Lclzr.Registries;

namespace Lclzr
{
    public sealed class LocalizerBuilder
    {
        internal readonly List<IBlocksProvider> Providers = new List<IBlocksProvider>();

        private ILocalizerRegistry? _registry;
        
        public LocalizerBuilder WithProvider<TProvider>(TProvider provider) where TProvider : class, IBlocksProvider
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            Providers.Add(provider);
            return this;
        }

        public LocalizerBuilder WithRawFilesProvider(RawFilesBlocksProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Providers.Add(new RawFilesBlocksProvider(options));
            return this;
        }

        public LocalizerBuilder WithEmbeddedResourcesProvider(EmbeddedResourcesBlocksProviderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Providers.Add(new EmbeddedResourcesBlocksProvider(options));
            return this;
        }
        
        public ILocalizer Build()
        {
            var registry = _registry ?? BuildAndInitializeRegistry().GetAwaiter().GetResult();

            return new Localizer(registry);
        }

        public ILocalizer<T> Build<T>() where T : class
        {
            var registry = _registry ?? BuildAndInitializeRegistry().GetAwaiter().GetResult();

            return new Localizer<T>(registry);
        }

        public async Task<ILocalizer> BuildAsync()
        {
            var registry = _registry ?? await BuildAndInitializeRegistry();

            return new Localizer(registry);
        }

        public async Task<ILocalizer<T>> BuildAsync<T>() where T : class
        {
            var registry = _registry ?? await BuildAndInitializeRegistry();

            return new Localizer<T>(registry);
        }

        internal LocalizerBuilder WithRegistry<TRegistry>(TRegistry registry) where TRegistry : class, ILocalizerRegistry
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
            return this;
        }

        private async Task<ILocalizerRegistry> BuildAndInitializeRegistry()
        {
            var registry = new LocalizerRegistry(Providers);
            await registry.Initialize();

            return registry;
        }
    }
}