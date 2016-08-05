using System;

namespace Docker.PowerShell.Support
{
    internal sealed class LinuxConsoleStream : ConsoleStream
    {
        public LinuxConsoleStream(ConsoleDirection dir) : base(dir)
        {
            Stream = dir == ConsoleDirection.In ? Console.OpenStandardInput() : Console.OpenStandardOutput();
        }
    }
}