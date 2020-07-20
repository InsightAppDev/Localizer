using System;

namespace Insight.Localizer
{
    public class MissingLocalizationException : Exception
    {
        public MissingLocalizationException(string message) : base(message)
        {
        }
    }
}