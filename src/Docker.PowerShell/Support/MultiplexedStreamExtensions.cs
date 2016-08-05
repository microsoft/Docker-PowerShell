using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;

#if !NET46
using System.Runtime.InteropServices;
#endif

namespace Docker.PowerShell.Support
{
    internal static class MultiplexedStreamExtensions
    {
        private static readonly bool IsWindows =
#if NET46
                true;
#else
                RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif

        public static async Task CopyToConsoleAsync(this MultiplexedStream stream, bool tty, bool openStdin, CancellationToken cancellationToken)
        {
            Stream stdin = Stream.Null, stdout = Stream.Null, stderr = Stream.Null;
            ConsoleStream conin = null, conout = null;
            Task stdinReadTask = null;
            try
            {
                // TODO: What if we are not attached to a console? If config.Tty is false, this should not be an error.

                if (IsWindows)
                {
                    conout = new Win32ConsoleStream(ConsoleDirection.Out);
                }
                else
                {
                    conout = new LinuxConsoleStream(ConsoleDirection.Out);
                }
                stdout = Console.OpenStandardOutput(); // Don't use conout's Stream because FileStream always buffers on net46.
                if (tty)
                {
                    conout.EnableVTMode();
                }
                else
                {
                    stderr = Console.OpenStandardError();
                }

                if (openStdin)
                {
                    if (IsWindows)
                    {
                        conin = new Win32ConsoleStream(ConsoleDirection.In);
                    }
                    else
                    {
                        conin = new LinuxConsoleStream(ConsoleDirection.In);
                    }

                    stdin = conin.Stream;
                    conin.EnableRawInputMode();
                    if (tty)
                    {
                        conin.EnableVTMode();
                    }

                    stdinReadTask = stream.CopyFromAsync(stdin, CancellationToken.None).ContinueWith(x => stream.CloseWrite());
                }

                await stream.CopyOutputToAsync(stdout, stdout, stderr, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                conout?.Dispose();
                conin?.Dispose();
                stdout.Dispose();
                stderr.Dispose();
                stdin.Dispose();
                if (stdinReadTask != null)
                {
                    try
                    {
                        // Make sure the read from stdin has finished before returning.
                        await stdinReadTask.ConfigureAwait(false);
                    }
                    catch (ObjectDisposedException)
                    {
                        // Ignore.
                    }
                }
            }
        }
    }
}