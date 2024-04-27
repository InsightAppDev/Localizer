using System;

namespace Lclzr.Exceptions
{
    public class MissingBlockException : Exception
    {
        public MissingBlockException(string message) : base(message)
        {
        }
    }
}