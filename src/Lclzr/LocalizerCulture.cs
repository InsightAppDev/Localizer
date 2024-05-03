using System;

namespace Lclzr
{
    public sealed class LocalizerCulture : ILocalizerCulture
    {
        public LocalizerCulture(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        public string Value { get; }
    }
}