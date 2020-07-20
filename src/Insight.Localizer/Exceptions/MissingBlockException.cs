using System;

namespace Insight.Localizer
{
    public class MissingBlockException : Exception
    {
        public MissingBlockException(string message) : base(message)
        {
        }
    }
}