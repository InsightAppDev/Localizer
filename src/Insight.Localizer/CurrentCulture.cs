using System;

namespace Insight.Localizer
{
    public sealed class CurrentCulture : ICurrentCulture
    {
        public CurrentCulture(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value));

            Value = value;
        }

        public string Value { get; }
    }

    public static class AnyCulture
    {
        public static string Value = "any";
    }
}