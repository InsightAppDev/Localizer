using System;
using System.Collections.Generic;
using Lclzr.Registries;

namespace Lclzr
{
    public class Localizer : ILocalizer
    {
        private readonly ILocalizerRegistry _registry;

        public ILocalizerCulture? CurrentCulture { get; set; }

        public IReadOnlyDictionary<string, Block> Blocks => _registry.Blocks;

        public IReadOnlyCollection<string> AvailableBlockNames => _registry.AvailableBlockNames;

        public Localizer(ILocalizerRegistry registry)
        {
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public Localizer(ILocalizerRegistry registry, ILocalizerCulture localizerCulture) : this(registry)
        {
            CurrentCulture = localizerCulture ?? throw new ArgumentNullException(nameof(localizerCulture));
        }

        public string Get(string block, string key)
        {
            if (CurrentCulture == null)
            {
                throw new InvalidOperationException($"{nameof(CurrentCulture)} is null");
            }
            
            return _registry.GetByCulture(CurrentCulture.Value, block, key);
        }

        public string GetAny(string block, string key)
        {
            return _registry.GetAny(block, key);
        }

        public string GetByCulture(string culture, string block, string key)
        {
            return _registry.GetByCulture(culture, block, key);
        }
    }
}