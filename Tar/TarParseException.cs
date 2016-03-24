using System;

namespace Tar
{
    class TarParseException : Exception
    {
        public TarParseException(string message) : base(message)
        {
        }
    }
}
