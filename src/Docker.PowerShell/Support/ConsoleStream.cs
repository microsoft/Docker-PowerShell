using System;
using System.IO;

namespace Docker.PowerShell.Support
{
    internal enum ConsoleDirection
    {
        In,
        Out,
    }

    internal abstract class ConsoleStream : IDisposable
    {
        protected IntPtr _handle;
        protected int _oldMode;
        protected int _currentMode;
        public Stream Stream { get; internal set; }
        public ConsoleDirection Direction { get; internal set; }

        public ConsoleStream(ConsoleDirection dir)
        {
            Direction = dir;
        }

        public virtual void EnableRawInputMode()
        {
        }

        public virtual void EnableVTMode()
        {
        }

        public virtual void Dispose()
        {
            Stream.Dispose();
        }
    }
}