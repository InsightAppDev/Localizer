using System;

namespace Lclzr.Exceptions
{
    public class LocalizerException : Exception
    {
        public LocalizerException(string message) : base(message)
        {
        }

        public LocalizerException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}