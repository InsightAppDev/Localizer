using System;
using System.Collections.Generic;

namespace Insight.Localizer
{
    public sealed class Block
    {
        public string Name { get; }

        private readonly IDictionary<string, IDictionary<string, string>> _localizations;

        private Block()
        {
            _localizations = new Dictionary<string, IDictionary<string, string>>();
        }

        internal Block(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        internal void Add(string key, IDictionary<string, string> value)
        {
            _localizations.Add(key, value);
        }

        public string Get(string culture, string key)
        {
            if (!_localizations.TryGetValue(culture, out var localization))
                throw new MissingLocalizationException($"There is no `{culture}` culture");

            if (!localization.TryGetValue(key, out var value))
                throw new MissingLocalizationException($"There is no `{key}` key");

            return value;
        }
    }
}