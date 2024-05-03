using System;

namespace Lclzr.Exceptions
{
    public class MissingLocalizationException : Exception
    {
        public MissingLocalizationException(string message) : base(message)
        {
        }
    }
}