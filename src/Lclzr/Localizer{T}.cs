using System;
using System.Collections.Generic;

namespace Lclzr
{
    internal class Localizer<T> : ILocalizer<T> where T : class
    {
        private readonly ILocalizer _localizer;

        public ILocalizerCulture CurrentCulture
        {
            get => _localizer.CurrentCulture;
            set => _localizer.CurrentCulture = value;
        }

        public IReadOnlyCollection<string> AvailableBlockNames => _localizer.AvailableBlockNames;

        public Localizer(ILocalizer localizer)
        {
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        }

        public string Get(string key)
        {
            var block = GetBlockName();
            return Get(block, key);
        }

        public string GetAny(string key)
        {
            var block = GetBlockName();
            return GetAny(block, key);
        }

        public string GetByCulture(string culture, string key)
        {
            var block = GetBlockName();
            return GetByCulture(culture, block, key);
        }

        public string Get(string block, string key)
        {
            return _localizer.Get(block, key);
        }

        public string GetAny(string block, string key)
        {
            return _localizer.GetAny(block, key);
        }

        public string GetByCulture(string culture, string block, string key)
        {
            return _localizer.GetByCulture(culture, block, key);
        }

        private static string GetBlockName()
        {
            return typeof(T).Name;
        }
    }
}