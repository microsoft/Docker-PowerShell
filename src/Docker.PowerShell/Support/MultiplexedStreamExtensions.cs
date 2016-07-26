using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet;

namespace Docker.PowerShell.Support
{
    internal static class MultiplexedStreamExtensions
    {
        public static async Task CopyToConsoleAsync(this MultiplexedStream stream, bool tty, bool openStdin, CancellationToken cancellationToken)
        {
            Stream stdin = Stream.Null, stdout = Stream.Null, stderr = Stream.Null;
            ConsoleStream conin = null, conout = null;
            try
            {
                // TODO: What if we are not attached to a console? If config.Tty is false, this should not be an error.
                conout = new ConsoleStream(ConsoleDirection.Out);
                stdout = Console.OpenStandardOutput(); // Don't use conout's Stream because FileStream always buffers on net46.
                if (tty)
                {
                    conout.EnableVTMode();
                }
                else
                {
                    stderr = Console.OpenStandardError();
                }

                Task stdinRead = null;
                CancellationTokenSource inputCancelToken = null;
                if (openStdin)
                {
                    conin = new ConsoleStream(ConsoleDirection.In);
                    stdin = conin.Stream;
                    conin.EnableRawInputMode();
                    if (tty)
                    {
                        conin.EnableVTMode();
                    }

                    inputCancelToken = new CancellationTokenSource();
                    stdinRead = stream.CopyFromAsync(stdin, inputCancelToken.Token).ContinueWith(x => stream.CloseWrite());
                }

                await stream.CopyOutputToAsync(stdout, stdout, stderr, cancellationToken).ConfigureAwait(false);

                if (stdinRead != null)
                {
                    inputCancelToken.Cancel();
                    try
                    {
                        await stdinRead.ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        // Ignore.
                    }
                }
            }
            finally
            {
                conin?.Dispose();
                conout?.Dispose();
                stdin.Dispose();
                stdout.Dispose();
                stderr.Dispose();
            }
        }
    }
}